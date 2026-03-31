using InputBroadcaster.Core;

namespace InputBroadcaster.Windows;

public sealed class InMemoryWindowRegistry : IWindowRegistry
{
    public WindowDescriptor? LeaderWindow { get; private set; }
    public WindowDescriptor? FollowerWindow { get; private set; }

    public void SetLeader(WindowDescriptor? window)
    {
        LeaderWindow = window;
    }

    public void SetFollower(WindowDescriptor? window)
    {
        FollowerWindow = window;
    }
}
