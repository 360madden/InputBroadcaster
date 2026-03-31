namespace InputBroadcaster.Input;

public sealed record KeyboardCapturePollResult(
    bool IsLeaderForeground,
    IReadOnlyList<RawKeyboardEvent> Events);
