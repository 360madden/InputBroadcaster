using System.Runtime.InteropServices;

namespace InputBroadcaster.Input;

internal static partial class Win32InputNativeMethods
{
    [LibraryImport("user32.dll")]
    internal static partial short GetAsyncKeyState(int vKey);

    [LibraryImport("user32.dll")]
    internal static partial nint GetForegroundWindow();
}
