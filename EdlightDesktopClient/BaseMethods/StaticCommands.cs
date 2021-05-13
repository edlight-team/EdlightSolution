using System.Windows;

namespace EdlightDesktopClient.BaseMethods
{
    internal static class StaticCommands
    {
        internal static void Shutdown() => Application.Current.Shutdown();
        internal static WindowState ChangeWindowState(WindowState state) => state == WindowState.Normal ? WindowState.Minimized : WindowState.Normal;
    }
}
