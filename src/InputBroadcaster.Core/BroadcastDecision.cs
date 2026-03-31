namespace InputBroadcaster.Core;

public sealed record BroadcastDecision(
    bool IsAllowed,
    string Reason,
    WindowDescriptor? TargetWindow);
