// Version: 0.1.0
// Total Characters: 1232
// Purpose: Resolve the first top-level window that matches typed process-name and title contains filters.

using InputBroadcaster.Core;

namespace InputBroadcaster.Windows;

public sealed class WindowMatcher
{
    public WindowDescriptor? FindFirstMatch(
        IEnumerable<WindowDescriptor> windows,
        string? processNameContains,
        string? titleContains)
    {
        var normalizedProcessFilter = Normalize(processNameContains);
        var normalizedTitleFilter = Normalize(titleContains);

        if (normalizedProcessFilter is null && normalizedTitleFilter is null)
        {
            return null;
        }

        return windows.FirstOrDefault(window =>
            MatchesContains(window.ProcessName, normalizedProcessFilter) &&
            MatchesContains(window.Title, normalizedTitleFilter));
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private static bool MatchesContains(string source, string? filter)
    {
        return filter is null || source.Contains(filter, StringComparison.OrdinalIgnoreCase);
    }
}

// End of file.
