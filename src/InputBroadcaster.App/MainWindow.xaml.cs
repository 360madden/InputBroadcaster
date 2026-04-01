// Version: 0.1.0
// Total Characters: 14528
// Purpose: Drive the MVP WPF shell by enumerating windows, selecting leader and follower windows directly or by typed match rules, polling leader-window keys, evaluating policy, and broadcasting allowed keys to the follower window.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using InputBroadcaster.Core;
using InputBroadcaster.Input;
using InputBroadcaster.Routing;
using InputBroadcaster.Sending;
using InputBroadcaster.Windows;

namespace InputBroadcaster.App;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<WindowChoice> _windowChoices = new();
    private readonly ObservableCollection<string> _logEntries = new();
    private readonly IWindowEnumerator _windowEnumerator = new Win32WindowEnumerator();
    private readonly IWindowRegistry _windowRegistry = new InMemoryWindowRegistry();
    private readonly LeaderWindowPollingKeyboardCapture _keyboardCapture = new();
    private readonly KeyboardEventNormalizer _keyboardEventNormalizer = new();
    private readonly KeyboardStateTracker _keyboardStateTracker = new();
    private readonly IBroadcastPolicyEvaluator _broadcastPolicyEvaluator = new AllowlistBroadcastPolicyEvaluator();
    private readonly IInputSender _inputSender = new Win32MessageInputSender();
    private readonly WindowMatcher _windowMatcher = new();
    private readonly DispatcherTimer _captureTimer;

    private IReadOnlyList<WindowDescriptor> _lastEnumeratedWindows = Array.Empty<WindowDescriptor>();
    private bool _isBroadcastEnabled;
    private bool _isCapturePausedByFocus;
    private bool _wasLeaderForegroundOnPreviousPoll = true;
    private bool _captureTickInProgress;

    public MainWindow()
    {
        InitializeComponent();

        LeaderComboBox.ItemsSource = _windowChoices;
        FollowerComboBox.ItemsSource = _windowChoices;
        LogListBox.ItemsSource = _logEntries;

        _captureTimer = new DispatcherTimer(DispatcherPriority.Background)
        {
            Interval = TimeSpan.FromMilliseconds(20),
        };
        _captureTimer.Tick += CaptureTimer_OnTick;

        RefreshWindows();
        UpdateStatus();
    }

    private void RefreshWindowsButton_OnClick(object sender, RoutedEventArgs e)
    {
        RefreshWindows();
    }

    private void SetLeaderButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (LeaderComboBox.SelectedItem is not WindowChoice selection)
        {
            AppendLog("WARN  No leader window is selected.");
            return;
        }

        _windowRegistry.SetLeader(selection.Descriptor);
        _keyboardCapture.Prime(selection.Descriptor.Handle);
        AppendLog($"INFO  Leader set to {selection.DisplayName}");
        UpdateStatus();
    }

    private void SetFollowerButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (FollowerComboBox.SelectedItem is not WindowChoice selection)
        {
            AppendLog("WARN  No follower window is selected.");
            return;
        }

        _windowRegistry.SetFollower(selection.Descriptor);
        AppendLog($"INFO  Follower set to {selection.DisplayName}");
        UpdateStatus();
    }

    private void FindLeaderMatchButton_OnClick(object sender, RoutedEventArgs e)
    {
        FindAndAssignWindow(
            LeaderProcessFilterTextBox.Text,
            LeaderTitleFilterTextBox.Text,
            LeaderComboBox,
            _windowRegistry.SetLeader,
            "leader");
    }

    private void FindFollowerMatchButton_OnClick(object sender, RoutedEventArgs e)
    {
        FindAndAssignWindow(
            FollowerProcessFilterTextBox.Text,
            FollowerTitleFilterTextBox.Text,
            FollowerComboBox,
            _windowRegistry.SetFollower,
            "follower");
    }

    private void StartBroadcastButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isBroadcastEnabled)
        {
            AppendLog("WARN  Broadcast is already running.");
            return;
        }

        RefreshWindows(appendLog: false);

        var leaderWindow = ResolveWindowForStart(
            "leader",
            _windowRegistry.LeaderWindow,
            LeaderProcessFilterTextBox.Text,
            LeaderTitleFilterTextBox.Text,
            LeaderComboBox,
            _windowRegistry.SetLeader);

        if (leaderWindow is null)
        {
            return;
        }

        var followerWindow = ResolveWindowForStart(
            "follower",
            _windowRegistry.FollowerWindow,
            FollowerProcessFilterTextBox.Text,
            FollowerTitleFilterTextBox.Text,
            FollowerComboBox,
            _windowRegistry.SetFollower);

        if (followerWindow is null)
        {
            return;
        }

        if (leaderWindow.Handle == followerWindow.Handle)
        {
            AppendLog("WARN  Cannot start. Leader and follower must be different windows.");
            return;
        }

        _keyboardStateTracker.Reset();
        _keyboardCapture.Prime(leaderWindow.Handle);
        _isBroadcastEnabled = true;
        _isCapturePausedByFocus = false;
        _wasLeaderForegroundOnPreviousPoll = true;
        _captureTimer.Start();

        AppendLog("INFO  Broadcast state changed to STARTED.");
        UpdateStatus();
    }

    private async void StopBroadcastButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_isBroadcastEnabled)
        {
            AppendLog("WARN  Broadcast is already stopped.");
            return;
        }

        _captureTimer.Stop();
        _isBroadcastEnabled = false;
        _isCapturePausedByFocus = false;
        _wasLeaderForegroundOnPreviousPoll = true;

        await ReleaseActiveKeysAsync("Broadcast stopped. Active follower keys released.");
        AppendLog("INFO  Broadcast state changed to STOPPED.");
        UpdateStatus();
    }

    private async void CaptureTimer_OnTick(object? sender, EventArgs e)
    {
        if (!_isBroadcastEnabled || _captureTickInProgress)
        {
            return;
        }

        var leaderWindow = _windowRegistry.LeaderWindow;
        var followerWindow = _windowRegistry.FollowerWindow;
        if (leaderWindow is null || followerWindow is null)
        {
            return;
        }

        _captureTickInProgress = true;

        try
        {
            var captureResult = _keyboardCapture.Poll(leaderWindow.Handle);

            if (!captureResult.IsLeaderForeground)
            {
                _isCapturePausedByFocus = true;

                if (_wasLeaderForegroundOnPreviousPoll)
                {
                    await ReleaseActiveKeysAsync("Leader window left the foreground. Active follower keys released.");
                    AppendLog("INFO  Capture paused because the leader window is not foreground.");
                }

                _wasLeaderForegroundOnPreviousPoll = false;
                UpdateStatus();
                return;
            }

            if (!_wasLeaderForegroundOnPreviousPoll)
            {
                _keyboardCapture.Prime(leaderWindow.Handle);
                _isCapturePausedByFocus = false;
                _wasLeaderForegroundOnPreviousPoll = true;
                AppendLog("INFO  Leader window returned to foreground. Capture resumed.");
                UpdateStatus();
                return;
            }

            _isCapturePausedByFocus = false;
            _wasLeaderForegroundOnPreviousPoll = true;

            foreach (var rawEvent in captureResult.Events)
            {
                var keyEvent = _keyboardEventNormalizer.Normalize(rawEvent);
                var decision = _broadcastPolicyEvaluator.Evaluate(keyEvent, DefaultPolicies.V01, followerWindow);

                if (!decision.IsAllowed)
                {
                    continue;
                }

                try
                {
                    await _inputSender.SendAsync(keyEvent, followerWindow, CancellationToken.None);
                    _keyboardStateTracker.Apply(keyEvent);
                }
                catch (Exception ex)
                {
                    AppendLog($"ERROR Failed to send {keyEvent.Key}: {ex.Message}");
                }
            }

            UpdateStatus();
        }
        finally
        {
            _captureTickInProgress = false;
        }
    }

    private void RefreshWindows(bool appendLog = true)
    {
        _lastEnumeratedWindows = _windowEnumerator
            .GetTopLevelWindows()
            .Where(window => window.Handle != 0)
            .ToArray();

        _windowChoices.Clear();
        foreach (var window in _lastEnumeratedWindows)
        {
            _windowChoices.Add(new WindowChoice(window));
        }

        if (appendLog)
        {
            AppendLog($"INFO  Enumerated {_windowChoices.Count} top-level windows.");
        }

        ReselectWindowChoices();
        UpdateStatus();
    }

    private void ReselectWindowChoices()
    {
        SelectChoice(LeaderComboBox, _windowRegistry.LeaderWindow);
        SelectChoice(FollowerComboBox, _windowRegistry.FollowerWindow);
    }

    private bool FindAndAssignWindow(
        string? processFilter,
        string? titleFilter,
        ComboBox comboBox,
        Action<WindowDescriptor?> assignWindow,
        string role)
    {
        RefreshWindows(appendLog: false);

        var match = _windowMatcher.FindFirstMatch(_lastEnumeratedWindows, processFilter, titleFilter);
        if (match is null)
        {
            AppendLog($"WARN  No {role} window matched the typed process/title filter.");
            return false;
        }

        assignWindow(match);
        SelectChoice(comboBox, match);

        if (string.Equals(role, "leader", StringComparison.OrdinalIgnoreCase))
        {
            _keyboardCapture.Prime(match.Handle);
        }

        AppendLog($"INFO  {Capitalize(role)} match resolved to {DescribeWindow(match)}");
        UpdateStatus();
        return true;
    }

    private WindowDescriptor? ResolveWindowForStart(
        string role,
        WindowDescriptor? currentWindow,
        string? processFilter,
        string? titleFilter,
        ComboBox comboBox,
        Action<WindowDescriptor?> assignWindow)
    {
        if (!string.IsNullOrWhiteSpace(processFilter) || !string.IsNullOrWhiteSpace(titleFilter))
        {
            var match = _windowMatcher.FindFirstMatch(_lastEnumeratedWindows, processFilter, titleFilter);
            if (match is null)
            {
                AppendLog($"WARN  Cannot start. No {role} window matched the configured process/title filter.");
                return null;
            }

            assignWindow(match);
            SelectChoice(comboBox, match);
            return match;
        }

        if (currentWindow is null)
        {
            AppendLog($"WARN  Cannot start. {Capitalize(role)} window is not set.");
            return null;
        }

        var liveWindow = _lastEnumeratedWindows.FirstOrDefault(window => window.Handle == currentWindow.Handle);
        if (liveWindow is null)
        {
            AppendLog($"WARN  Cannot start. Selected {role} window is no longer available.");
            return null;
        }

        assignWindow(liveWindow);
        SelectChoice(comboBox, liveWindow);
        return liveWindow;
    }

    private static void SelectChoice(ComboBox comboBox, WindowDescriptor? window)
    {
        if (window is null)
        {
            comboBox.SelectedItem = null;
            return;
        }

        comboBox.SelectedItem = comboBox.Items
            .OfType<WindowChoice>()
            .FirstOrDefault(choice => choice.Descriptor.Handle == window.Handle);
    }

    private async Task ReleaseActiveKeysAsync(string? reason = null)
    {
        var followerWindow = _windowRegistry.FollowerWindow;
        var leaderHandle = _windowRegistry.LeaderWindow?.Handle ?? 0;
        var activeKeys = _keyboardStateTracker.ActiveKeys.ToArray();

        if (followerWindow is not null)
        {
            foreach (var key in activeKeys)
            {
                var keyUpEvent = new BroadcastKeyEvent(
                    key,
                    IsKeyDown: false,
                    IsKeyUp: true,
                    ShiftActive: false,
                    CtrlActive: false,
                    AltActive: false,
                    SourceWindowHandle: leaderHandle,
                    TimestampUtc: DateTimeOffset.UtcNow);

                try
                {
                    await _inputSender.SendAsync(keyUpEvent, followerWindow, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    AppendLog($"ERROR Failed to release {key}: {ex.Message}");
                }
            }
        }

        if (activeKeys.Length > 0 && !string.IsNullOrWhiteSpace(reason))
        {
            AppendLog($"INFO  {reason}");
        }

        _keyboardStateTracker.Reset();
        _keyboardCapture.Reset();
    }

    private void UpdateStatus()
    {
        var state = !_isBroadcastEnabled
            ? "Idle"
            : _isCapturePausedByFocus
                ? "Paused (leader not foreground)"
                : "Running";

        var leader = DescribeWindow(_windowRegistry.LeaderWindow);
        var follower = DescribeWindow(_windowRegistry.FollowerWindow);
        StatusTextBlock.Text = $"State: {state} | Leader: {leader} | Follower: {follower}";
    }

    private void AppendLog(string message)
    {
        _logEntries.Insert(0, $"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss}] {message}");
    }

    private static string DescribeWindow(WindowDescriptor? window)
    {
        return window is null
            ? "not set"
            : $"{window.ProcessName} | {window.Title}";
    }

    private static string Capitalize(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? value
            : string.Concat(char.ToUpperInvariant(value[0]), value[1..]);
    }

    private sealed class WindowChoice
    {
        public WindowChoice(WindowDescriptor descriptor)
        {
            Descriptor = descriptor;
            DisplayName = $"{descriptor.ProcessName} | {descriptor.Title}";
        }

        public WindowDescriptor Descriptor { get; }

        public string DisplayName { get; }
    }
}

// End of file.
