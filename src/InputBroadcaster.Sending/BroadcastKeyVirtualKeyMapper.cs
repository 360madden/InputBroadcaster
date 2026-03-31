using InputBroadcaster.Core;

namespace InputBroadcaster.Sending;

internal static class BroadcastKeyVirtualKeyMapper
{
    internal static uint Map(BroadcastKey key)
    {
        return key switch
        {
            BroadcastKey.D1 => 0x31,
            BroadcastKey.D2 => 0x32,
            BroadcastKey.D3 => 0x33,
            BroadcastKey.D4 => 0x34,
            BroadcastKey.D5 => 0x35,
            BroadcastKey.D6 => 0x36,
            BroadcastKey.D7 => 0x37,
            BroadcastKey.D8 => 0x38,
            BroadcastKey.D9 => 0x39,
            BroadcastKey.D0 => 0x30,
            BroadcastKey.OemMinus => 0xBD,
            BroadcastKey.OemPlus => 0xBB,
            _ => 0,
        };
    }
}
