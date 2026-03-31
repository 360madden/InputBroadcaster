namespace InputBroadcaster.Input;

public sealed class LeaderWindowPollingKeyboardCapture
{
    private static readonly int[] ObservedVirtualKeys =
    {
        0x31,
        0x32,
        0x33,
        0x34,
        0x35,
        0x36,
        0x37,
        0x38,
        0x39,
        0x30,
        0xBD,
        0xBB,
    };

    private const int VkShift = 0x10;
    private const int VkControl = 0x11;
    private const int VkMenu = 0x12;

    private readonly Dictionary<int, bool> _previousStates = new();

    public KeyboardCapturePollResult Poll(nint leaderWindowHandle)
    {
        if (leaderWindowHandle == 0)
        {
            return new KeyboardCapturePollResult(false, Array.Empty<RawKeyboardEvent>());
        }

        var foregroundWindow = Win32InputNativeMethods.GetForegroundWindow();
        if (foregroundWindow != leaderWindowHandle)
        {
            return new KeyboardCapturePollResult(false, Array.Empty<RawKeyboardEvent>());
        }

        var timestampUtc = DateTimeOffset.UtcNow;
        var shiftActive = IsKeyCurrentlyDown(VkShift);
        var ctrlActive = IsKeyCurrentlyDown(VkControl);
        var altActive = IsKeyCurrentlyDown(VkMenu);
        var events = new List<RawKeyboardEvent>();

        foreach (var virtualKeyCode in ObservedVirtualKeys)
        {
            var isCurrentlyDown = IsKeyCurrentlyDown(virtualKeyCode);
            var wasPreviouslyDown = _previousStates.TryGetValue(virtualKeyCode, out var previousState) && previousState;

            if (isCurrentlyDown == wasPreviouslyDown)
            {
                continue;
            }

            _previousStates[virtualKeyCode] = isCurrentlyDown;
            events.Add(new RawKeyboardEvent(
                virtualKeyCode,
                IsKeyDown: isCurrentlyDown,
                IsKeyUp: !isCurrentlyDown,
                ShiftActive: shiftActive,
                CtrlActive: ctrlActive,
                AltActive: altActive,
                SourceWindowHandle: leaderWindowHandle,
                TimestampUtc: timestampUtc));
        }

        return new KeyboardCapturePollResult(true, events);
    }

    public void Prime(nint leaderWindowHandle)
    {
        _previousStates.Clear();

        if (leaderWindowHandle == 0)
        {
            return;
        }

        if (Win32InputNativeMethods.GetForegroundWindow() != leaderWindowHandle)
        {
            return;
        }

        foreach (var virtualKeyCode in ObservedVirtualKeys)
        {
            _previousStates[virtualKeyCode] = IsKeyCurrentlyDown(virtualKeyCode);
        }
    }

    public void Reset()
    {
        _previousStates.Clear();
    }

    private static bool IsKeyCurrentlyDown(int virtualKeyCode)
    {
        return (Win32InputNativeMethods.GetAsyncKeyState(virtualKeyCode) & 0x8000) != 0;
    }
}
