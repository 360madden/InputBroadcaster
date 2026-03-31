namespace InputBroadcaster.Core;

public interface IWindowRegistry
{
    WindowDescriptor? LeaderWindow { get; }
    WindowDescriptor? FollowerWindow { get; }
    void SetLeader(WindowDescriptor? window);
    void SetFollower(WindowDescriptor? window);
}
