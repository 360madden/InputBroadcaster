namespace InputBroadcaster.Core;

public sealed record BroadcastKeyEvent(
    BroadcastKey Key,
    bool IsKeyDown,
    bool IsKeyUp,
    bool ShiftActive,
    bool CtrlActive,
    bool AltActive,
    nint SourceWindowHandle,
    DateTimeOffset TimestampUtc)
{
    public bool HasModifier => ShiftActive || CtrlActive || AltActive;
}
