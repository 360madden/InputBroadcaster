namespace InputBroadcaster.Core;

public static class DefaultPolicies
{
    public static BroadcastPolicy V01 { get; } = new(
        new HashSet<BroadcastKey>
        {
            BroadcastKey.D1,
            BroadcastKey.D2,
            BroadcastKey.D3,
            BroadcastKey.D4,
            BroadcastKey.D5,
            BroadcastKey.D6,
            BroadcastKey.D7,
            BroadcastKey.D8,
            BroadcastKey.D9,
            BroadcastKey.D0,
            BroadcastKey.OemMinus,
            BroadcastKey.OemPlus,
        },
        ModifiersAllowed: false,
        MouseEnabled: false);
}
