namespace InputBroadcaster.Core;

public interface IKeyboardEventSource
{
    event EventHandler<RawKeyboardEvent>? KeyboardEventReceived;
}
