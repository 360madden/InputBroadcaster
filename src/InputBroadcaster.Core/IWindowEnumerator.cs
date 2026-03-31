namespace InputBroadcaster.Core;

public interface IWindowEnumerator
{
    IReadOnlyList<WindowDescriptor> GetTopLevelWindows();
}
