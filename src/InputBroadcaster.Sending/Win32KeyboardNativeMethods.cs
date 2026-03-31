using System.Runtime.InteropServices;

namespace InputBroadcaster.Sending;

internal static partial class Win32KeyboardNativeMethods
{
    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool PostMessage(nint hWnd, uint msg, nuint wParam, nint lParam);

    [LibraryImport("user32.dll")]
    internal static partial uint MapVirtualKey(uint uCode, uint uMapType);
}
