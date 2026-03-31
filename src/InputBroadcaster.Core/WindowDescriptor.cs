namespace InputBroadcaster.Core;

public sealed record WindowDescriptor(
    nint Handle,
    string Title,
    string ProcessName,
    bool IsEnabled);
