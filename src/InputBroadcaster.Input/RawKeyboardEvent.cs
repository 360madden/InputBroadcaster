namespace InputBroadcaster.Input;

public sealed record RawKeyboardEvent(
    int VirtualKeyCode,
    bool IsKeyDown,
    bool IsKeyUp,
    bool ShiftActive,
    bool CtrlActive,
    bool AltActive,
    nint SourceWindowHandle,
    DateTimeOffset TimestampUtc);
