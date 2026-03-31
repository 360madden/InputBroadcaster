using InputBroadcaster.Core;

namespace InputBroadcaster.Input;

public sealed class KeyboardEventNormalizer
{
    public BroadcastKeyEvent Normalize(RawKeyboardEvent rawEvent)
    {
        return new BroadcastKeyEvent(
            MapKey(rawEvent.VirtualKeyCode),
            rawEvent.IsKeyDown,
            rawEvent.IsKeyUp,
            rawEvent.ShiftActive,
            rawEvent.CtrlActive,
            rawEvent.AltActive,
            rawEvent.SourceWindowHandle,
            rawEvent.TimestampUtc);
    }

    private static BroadcastKey MapKey(int virtualKeyCode)
    {
        return virtualKeyCode switch
        {
            0x31 => BroadcastKey.D1,
            0x32 => BroadcastKey.D2,
            0x33 => BroadcastKey.D3,
            0x34 => BroadcastKey.D4,
            0x35 => BroadcastKey.D5,
            0x36 => BroadcastKey.D6,
            0x37 => BroadcastKey.D7,
            0x38 => BroadcastKey.D8,
            0x39 => BroadcastKey.D9,
            0x30 => BroadcastKey.D0,
            0xBD => BroadcastKey.OemMinus,
            0xBB => BroadcastKey.OemPlus,
            _ => BroadcastKey.Unknown,
        };
    }
}
