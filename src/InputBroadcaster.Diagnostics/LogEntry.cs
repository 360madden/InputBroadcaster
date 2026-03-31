namespace InputBroadcaster.Diagnostics;

public sealed record LogEntry(DateTimeOffset TimestampUtc, string Level, string Message);
