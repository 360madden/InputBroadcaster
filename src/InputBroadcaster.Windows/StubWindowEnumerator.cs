using InputBroadcaster.Core;

namespace InputBroadcaster.Windows;

public sealed class StubWindowEnumerator : IWindowEnumerator
{
    public IReadOnlyList<WindowDescriptor> GetTopLevelWindows()
    {
        return Array.Empty<WindowDescriptor>();
    }
}
