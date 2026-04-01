// Version: 0.1.0
// Total Characters: 2036
// Purpose: Lock typed process/title window matching behavior so leader and follower resolution stays deterministic.

using InputBroadcaster.Core;
using InputBroadcaster.Windows;
using Xunit;

namespace InputBroadcaster.Tests;

public sealed class WindowMatcherTests
{
    private static readonly WindowDescriptor RiftLeader = new((nint)101, "RIFT - Leader", "rift_x64", true);
    private static readonly WindowDescriptor RiftFollower = new((nint)102, "RIFT - Follower", "rift_x64", true);
    private static readonly WindowDescriptor Notepad = new((nint)103, "notes.txt - Notepad", "notepad", true);

    [Fact]
    public void FindFirstMatch_Returns_Null_When_No_Filter_Is_Provided()
    {
        var matcher = new WindowMatcher();

        var match = matcher.FindFirstMatch(new[] { RiftLeader, RiftFollower, Notepad }, "", "");

        Assert.Null(match);
    }

    [Fact]
    public void FindFirstMatch_Can_Match_By_Process_Name()
    {
        var matcher = new WindowMatcher();

        var match = matcher.FindFirstMatch(new[] { Notepad, RiftLeader }, "rift_x64", null);

        Assert.Equal(RiftLeader, match);
    }

    [Fact]
    public void FindFirstMatch_Can_Match_By_Title()
    {
        var matcher = new WindowMatcher();

        var match = matcher.FindFirstMatch(new[] { RiftFollower, RiftLeader }, null, "Leader");

        Assert.Equal(RiftLeader, match);
    }

    [Fact]
    public void FindFirstMatch_Can_Combine_Process_And_Title_Filters()
    {
        var matcher = new WindowMatcher();

        var match = matcher.FindFirstMatch(new[] { Notepad, RiftFollower, RiftLeader }, "rift_x64", "Follower");

        Assert.Equal(RiftFollower, match);
    }

    [Fact]
    public void FindFirstMatch_Does_Not_Match_Wrong_Process_When_Title_Alone_Is_Not_Enough()
    {
        var matcher = new WindowMatcher();

        var match = matcher.FindFirstMatch(new[] { Notepad, RiftLeader }, "rift_x64", "Notepad");

        Assert.Null(match);
    }
}

// End of file.
