using InputBroadcaster.Core;

namespace InputBroadcaster.Routing;

public sealed class AllowlistBroadcastPolicyEvaluator : IBroadcastPolicyEvaluator
{
    public BroadcastDecision Evaluate(BroadcastKeyEvent keyEvent, BroadcastPolicy policy, WindowDescriptor? targetWindow)
    {
        if (targetWindow is null || !targetWindow.IsEnabled)
        {
            return new BroadcastDecision(false, "Target window unavailable", null);
        }

        if (keyEvent.HasModifier && !policy.ModifiersAllowed)
        {
            return new BroadcastDecision(false, "Modifiers are not allowed in v0.1", targetWindow);
        }

        if (!policy.AllowedKeys.Contains(keyEvent.Key))
        {
            return new BroadcastDecision(false, "Key not in allowlist", targetWindow);
        }

        return new BroadcastDecision(true, "Allowed", targetWindow);
    }
}
