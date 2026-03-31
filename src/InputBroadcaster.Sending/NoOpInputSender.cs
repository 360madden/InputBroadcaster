using InputBroadcaster.Core;

namespace InputBroadcaster.Sending;

public sealed class NoOpInputSender : IInputSender
{
    public Task SendAsync(BroadcastKeyEvent keyEvent, WindowDescriptor targetWindow, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
