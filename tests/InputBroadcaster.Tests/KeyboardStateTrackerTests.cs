// Version: 0.1.0
// Total Characters: 1622
// Purpose: Lock keyboard state tracking behavior for key down, key up, and reset operations.

using System;
using InputBroadcaster.Core;
using InputBroadcaster.Input;
using Xunit;

namespace InputBroadcaster.Tests;

public sealed class KeyboardStateTrackerTests
{
    [Fact]
    public void Apply_Adds_Key_On_KeyDown()
    {
        var tracker = new KeyboardStateTracker();
        var keyEvent = CreateKeyEvent(isKeyDown: true, isKeyUp: false);

        tracker.Apply(keyEvent);

        Assert.Contains(BroadcastKey.D1, tracker.ActiveKeys);
    }

    [Fact]
    public void Apply_Removes_Key_On_KeyUp()
    {
        var tracker = new KeyboardStateTracker();
        tracker.Apply(CreateKeyEvent(isKeyDown: true, isKeyUp: false));

        tracker.Apply(CreateKeyEvent(isKeyDown: false, isKeyUp: true));

        Assert.DoesNotContain(BroadcastKey.D1, tracker.ActiveKeys);
    }

    [Fact]
    public void Reset_Clears_All_Tracked_Keys()
    {
        var tracker = new KeyboardStateTracker();
        tracker.Apply(CreateKeyEvent(isKeyDown: true, isKeyUp: false));

        tracker.Reset();

        Assert.Empty(tracker.ActiveKeys);
    }

    private static BroadcastKeyEvent CreateKeyEvent(bool isKeyDown, bool isKeyUp)
    {
        return new BroadcastKeyEvent(
            BroadcastKey.D1,
            IsKeyDown: isKeyDown,
            IsKeyUp: isKeyUp,
            ShiftActive: false,
            CtrlActive: false,
            AltActive: false,
            SourceWindowHandle: (nint)111,
            TimestampUtc: DateTimeOffset.UtcNow);
    }
}

// End of file.
