// Version: 0.1.0
// Total Characters: 5071
// Purpose: Drive the MVP WPF shell by enumerating windows, storing leader/follower selection, and exposing honest broadcast state.

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using InputBroadcaster.Core;
using InputBroadcaster.Windows;

namespace InputBroadcaster.App;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<WindowChoice> _windowChoices = new();
    private readonly ObservableCollection<string> _logEntries = new();
    private readonly IWindowEnumerator _windowEnumerator = new Win32WindowEnumerator();
    private readonly IWindowRegistry _windowRegistry = new InMemoryWindowRegistry();

    private bool _isBroadcastEnabled;

    public MainWindow()
    {
        InitializeComponent();

        LeaderComboBox.ItemsSource = _windowChoices;
        FollowerComboBox.ItemsSource = _windowChoices;
        LogListBox.ItemsSource = _logEntries;

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

    private void StartBroadcastButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_windowRegistry.LeaderWindow is null)
        {
            AppendLog("WARN  Cannot start. Leader window is not set.");
            return;
        }

        if (_windowRegistry.FollowerWindow is null)
        {
            AppendLog("WARN  Cannot start. Follower window is not set.");
            return;
        }

        if (_windowRegistry.LeaderWindow.Handle == _windowRegistry.FollowerWindow.Handle)
        {
            AppendLog("WARN  Cannot start. Leader and follower must be different windows.");
            return;
        }

        _isBroadcastEnabled = true;
        AppendLog("INFO  Broadcast state changed to STARTED.");
        UpdateStatus();
    }

    private void StopBroadcastButton_OnClick(object sender, RoutedEventArgs e)
    {
        _isBroadcastEnabled = false;
        AppendLog("INFO  Broadcast state changed to STOPPED.");
        UpdateStatus();
    }

    private void RefreshWindows()
    {
        var windows = _windowEnumerator
            .GetTopLevelWindows()
            .Where(window => window.Handle != 0)
            .ToArray();

        _windowChoices.Clear();
        foreach (var window in windows)
        {
            _windowChoices.Add(new WindowChoice(window));
        }

        AppendLog($"INFO  Enumerated {_windowChoices.Count} top-level windows.");
        ReselectWindowChoices();
        UpdateStatus();
    }

    private void ReselectWindowChoices()
    {
        SelectChoice(LeaderComboBox, _windowRegistry.LeaderWindow);
        SelectChoice(FollowerComboBox, _windowRegistry.FollowerWindow);
    }

    private static void SelectChoice(System.Windows.Controls.ComboBox comboBox, WindowDescriptor? window)
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

    private void UpdateStatus()
    {
        var leader = DescribeWindow(_windowRegistry.LeaderWindow);
        var follower = DescribeWindow(_windowRegistry.FollowerWindow);
        var state = _isBroadcastEnabled ? "Running" : "Idle";

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
