// Version: 0.1.0
// Total Characters: 1306
// Purpose: Lock the v0.1 default broadcast policy so future changes do not silently widen scope.

using InputBroadcaster.Core;
using Xunit;

namespace InputBroadcaster.Tests;

public sealed class DefaultPoliciesTests
{
    [Fact]
    public void V01_Has_Expected_Allowed_Keys_And_Flags()
    {
        var policy = DefaultPolicies.V01;

        Assert.False(policy.ModifiersAllowed);
        Assert.False(policy.MouseEnabled);

        Assert.Contains(BroadcastKey.D1, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D2, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D3, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D4, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D5, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D6, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D7, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D8, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D9, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.D0, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.OemMinus, policy.AllowedKeys);
        Assert.Contains(BroadcastKey.OemPlus, policy.AllowedKeys);

        Assert.Equal(12, policy.AllowedKeys.Count);
    }
}

// End of file.
