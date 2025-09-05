using System.Windows.Input;

namespace AINovelAgent.Commands.Definitions
{
    public static class EditCommands
    {
        public static readonly RoutedUICommand Undo = new("撤销", nameof(Undo), typeof(EditCommands));
        public static readonly RoutedUICommand Redo = new("重做", nameof(Redo), typeof(EditCommands));
        public static readonly RoutedUICommand Cut = new("剪切", nameof(Cut), typeof(EditCommands));
        public static readonly RoutedUICommand Copy = new("复制", nameof(Copy), typeof(EditCommands));
        public static readonly RoutedUICommand Paste = new("粘贴", nameof(Paste), typeof(EditCommands));
        public static readonly RoutedUICommand Find = new("查找", nameof(Find), typeof(EditCommands));
        public static readonly RoutedUICommand Replace = new("替换", nameof(Replace), typeof(EditCommands));
    }
}


