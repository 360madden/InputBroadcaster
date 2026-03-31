using System.ComponentModel;
using InputBroadcaster.Core;

namespace InputBroadcaster.Sending;

public sealed class Win32MessageInputSender : IInputSender
{
    private const uint WmKeyDown = 0x0100;
    private const uint WmKeyUp = 0x0101;

    public Task SendAsync(BroadcastKeyEvent keyEvent, WindowDescriptor targetWindow, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (targetWindow.Handle == 0)
        {
            throw new ArgumentException("Target window handle is required.", nameof(targetWindow));
        }

        var virtualKey = BroadcastKeyVirtualKeyMapper.Map(keyEvent.Key);
        if (virtualKey == 0)
        {
            return Task.CompletedTask;
        }

        var message = ResolveMessage(keyEvent);
        if (message == 0)
        {
            return Task.CompletedTask;
        }

        var scanCode = Win32KeyboardNativeMethods.MapVirtualKey(virtualKey, 0);
        var lParam = BuildLParam(scanCode, keyEvent.IsKeyUp);

        if (!Win32KeyboardNativeMethods.PostMessage(targetWindow.Handle, message, virtualKey, lParam))
        {
            throw new Win32Exception();
        }

        return Task.CompletedTask;
    }

    private static uint ResolveMessage(BroadcastKeyEvent keyEvent)
    {
        if (keyEvent.IsKeyDown)
        {
            return WmKeyDown;
        }

        if (keyEvent.IsKeyUp)
        {
            return WmKeyUp;
        }

        return 0;
    }

    private static nint BuildLParam(uint scanCode, bool isKeyUp)
    {
        var value = 1 | ((int)scanCode << 16);

        if (isKeyUp)
        {
            value |= 1 << 30;
            value = unchecked((int)((uint)value | 0x80000000));
        }

        return value;
    }
}
