using InputBroadcaster.Core;
using InputBroadcaster.Routing;

namespace InputBroadcaster.Tests;

public sealed class AllowlistBroadcastPolicyEvaluatorTests
{
    [Fact]
    public void Evaluate_Allows_Default_Key_Without_Modifiers()
    {
        var evaluator = new AllowlistBroadcastPolicyEvaluator();
        var keyEvent = new BroadcastKeyEvent(BroadcastKey.D1, true, false, false, false, false, 1, DateTimeOffset.UtcNow);
        var target = new WindowDescriptor(2, "Follower", "rift_x64", true);

        var result = evaluator.Evaluate(keyEvent, DefaultPolicies.V01, target);

        Assert.True(result.IsAllowed);
    }

    [Fact]
    public void Evaluate_Rejects_Modified_Key_In_V01()
    {
        var evaluator = new AllowlistBroadcastPolicyEvaluator();
        var keyEvent = new BroadcastKeyEvent(BroadcastKey.D1, true, false, true, false, false, 1, DateTimeOffset.UtcNow);
        var target = new WindowDescriptor(2, "Follower", "rift_x64", true);

        var result = evaluator.Evaluate(keyEvent, DefaultPolicies.V01, target);

        Assert.False(result.IsAllowed);
    }
}
