using InputBroadcaster.Core;

namespace InputBroadcaster.Input;

public sealed class KeyboardStateTracker
{
    private readonly HashSet<BroadcastKey> _activeKeys = new();

    public IReadOnlyCollection<BroadcastKey> ActiveKeys => _activeKeys;

    public void Apply(BroadcastKeyEvent keyEvent)
    {
        if (keyEvent.IsKeyDown)
        {
            _activeKeys.Add(keyEvent.Key);
        }

        if (keyEvent.IsKeyUp)
        {
            _activeKeys.Remove(keyEvent.Key);
        }
    }

    public void Reset()
    {
        _activeKeys.Clear();
    }
}
