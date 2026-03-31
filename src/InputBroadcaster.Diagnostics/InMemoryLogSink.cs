namespace InputBroadcaster.Diagnostics;

public sealed class InMemoryLogSink
{
    private readonly List<LogEntry> _entries = new();

    public IReadOnlyList<LogEntry> Entries => _entries;

    public void Write(string level, string message)
    {
        _entries.Add(new LogEntry(DateTimeOffset.UtcNow, level, message));
    }
}
