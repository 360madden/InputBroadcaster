// Version: 0.1.0
// Total Characters: 1626
// Purpose: Lock BroadcastKeyEvent modifier detection behavior used by the routing policy.

using System;
using InputBroadcaster.Core;
using Xunit;

namespace InputBroadcaster.Tests;

public sealed class BroadcastKeyEventTests
{
    [Fact]
    public void HasModifier_Is_False_When_No_Modifier_Is_Active()
    {
        var keyEvent = CreateKeyEvent(shiftActive: false, ctrlActive: false, altActive: false);

        Assert.False(keyEvent.HasModifier);
    }

    [Fact]
    public void HasModifier_Is_True_When_Shift_Is_Active()
    {
        var keyEvent = CreateKeyEvent(shiftActive: true, ctrlActive: false, altActive: false);

        Assert.True(keyEvent.HasModifier);
    }

    [Fact]
    public void HasModifier_Is_True_When_Ctrl_Is_Active()
    {
        var keyEvent = CreateKeyEvent(shiftActive: false, ctrlActive: true, altActive: false);

        Assert.True(keyEvent.HasModifier);
    }

    [Fact]
    public void HasModifier_Is_True_When_Alt_Is_Active()
    {
        var keyEvent = CreateKeyEvent(shiftActive: false, ctrlActive: false, altActive: true);

        Assert.True(keyEvent.HasModifier);
    }

    private static BroadcastKeyEvent CreateKeyEvent(bool shiftActive, bool ctrlActive, bool altActive)
    {
        return new BroadcastKeyEvent(
            BroadcastKey.D1,
            IsKeyDown: true,
            IsKeyUp: false,
            ShiftActive: shiftActive,
            CtrlActive: ctrlActive,
            AltActive: altActive,
            SourceWindowHandle: (nint)999,
            TimestampUtc: DateTimeOffset.UtcNow);
    }
}

// End of file.
