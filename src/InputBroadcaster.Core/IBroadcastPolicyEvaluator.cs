namespace InputBroadcaster.Core;

public interface IBroadcastPolicyEvaluator
{
    BroadcastDecision Evaluate(BroadcastKeyEvent keyEvent, BroadcastPolicy policy, WindowDescriptor? targetWindow);
}
