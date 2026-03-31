using InputBroadcaster.Core;

namespace InputBroadcaster.Configuration;

public sealed record AppConfiguration(
    WindowDescriptor? LeaderWindow,
    IReadOnlyList<WindowDescriptor> FollowerWindows,
    IReadOnlySet<BroadcastKey> AllowedKeys,
    bool IsBroadcastEnabled)
{
    public static AppConfiguration Default { get; } = new(
        null,
        Array.Empty<WindowDescriptor>(),
        new HashSet<BroadcastKey>(DefaultPolicies.V01.AllowedKeys),
        false);
}
