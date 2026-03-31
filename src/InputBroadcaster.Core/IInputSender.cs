namespace InputBroadcaster.Core;

public interface IInputSender
{
    Task SendAsync(BroadcastKeyEvent keyEvent, WindowDescriptor targetWindow, CancellationToken cancellationToken);
}
