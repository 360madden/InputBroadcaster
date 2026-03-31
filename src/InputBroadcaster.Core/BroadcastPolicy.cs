namespace InputBroadcaster.Core;

public sealed record BroadcastPolicy(
    IReadOnlySet<BroadcastKey> AllowedKeys,
    bool ModifiersAllowed,
    bool MouseEnabled);
