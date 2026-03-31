using System.Diagnostics;
using System.Text;
using InputBroadcaster.Core;

namespace InputBroadcaster.Windows;

public sealed class Win32WindowEnumerator : IWindowEnumerator
{
    public IReadOnlyList<WindowDescriptor> GetTopLevelWindows()
    {
        var windows = new List<WindowDescriptor>();

        Win32NativeMethods.EnumWindows(
            (hWnd, _) =>
            {
                if (!Win32NativeMethods.IsWindowVisible(hWnd))
                {
                    return true;
                }

                var titleLength = Win32NativeMethods.GetWindowTextLength(hWnd);
                if (titleLength <= 0)
                {
                    return true;
                }

                var titleBuilder = new StringBuilder(titleLength + 1);
                _ = Win32NativeMethods.GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);
                var title = titleBuilder.ToString().Trim();
                if (string.IsNullOrWhiteSpace(title))
                {
                    return true;
                }

                _ = Win32NativeMethods.GetWindowThreadProcessId(hWnd, out var processId);
                var processName = TryGetProcessName(processId);

                windows.Add(new WindowDescriptor(hWnd, title, processName, true));
                return true;
            },
            0);

        return windows
            .OrderBy(window => window.ProcessName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(window => window.Title, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static string TryGetProcessName(uint processId)
    {
        try
        {
            return Process.GetProcessById((int)processId).ProcessName;
        }
        catch
        {
            return "unknown";
        }
    }
}
