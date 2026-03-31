using InputBroadcaster.Core;
using InputBroadcaster.Input;

namespace InputBroadcaster.Tests;

public sealed class KeyboardEventNormalizerTests
{
    [Fact]
    public void Normalize_Maps_Allowed_Key()
    {
        var normalizer = new KeyboardEventNormalizer();
        var raw = new RawKeyboardEvent(0x31, true, false, false, false, false, 1, DateTimeOffset.UtcNow);

        var result = normalizer.Normalize(raw);

        Assert.Equal(BroadcastKey.D1, result.Key);
    }

    [Fact]
    public void Normalize_Maps_Unknown_Key_To_Unknown()
    {
        var normalizer = new KeyboardEventNormalizer();
        var raw = new RawKeyboardEvent(0x41, true, false, false, false, false, 1, DateTimeOffset.UtcNow);

        var result = normalizer.Normalize(raw);

        Assert.Equal(BroadcastKey.Unknown, result.Key);
    }
}
