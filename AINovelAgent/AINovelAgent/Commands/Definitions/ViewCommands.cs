using System.Windows.Input;

namespace AINovelAgent.Commands.Definitions
{
    public static class ViewCommands
    {
        public static readonly RoutedUICommand ToggleSidebar = new("侧边栏显示/隐藏", nameof(ToggleSidebar), typeof(ViewCommands));
        public static readonly RoutedUICommand ToggleToolbar = new("工具栏显示/隐藏", nameof(ToggleToolbar), typeof(ViewCommands));
        public static readonly RoutedUICommand ToggleStatusBar = new("状态栏显示/隐藏", nameof(ToggleStatusBar), typeof(ViewCommands));
        public static readonly RoutedUICommand Fullscreen = new("全屏模式", nameof(Fullscreen), typeof(ViewCommands));
    }
}


