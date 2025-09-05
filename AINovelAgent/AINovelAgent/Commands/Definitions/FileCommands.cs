using System.Windows.Input;

namespace AINovelAgent.Commands.Definitions
{
    public static class FileCommands
    {
        public static readonly RoutedUICommand New = new("新建", nameof(New), typeof(FileCommands));
        public static readonly RoutedUICommand Open = new("打开", nameof(Open), typeof(FileCommands));
        public static readonly RoutedUICommand Save = new("保存", nameof(Save), typeof(FileCommands));
        public static readonly RoutedUICommand SaveAs = new("另存为", nameof(SaveAs), typeof(FileCommands));
        public static readonly RoutedUICommand Exit = new("退出", nameof(Exit), typeof(FileCommands));
    }
}


