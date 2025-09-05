using System.Windows.Input;

namespace AINovelAgent.Commands.Definitions
{
    public static class HelpCommands
    {
        public static readonly RoutedUICommand OpenUserGuide = new("用户手册", nameof(OpenUserGuide), typeof(HelpCommands));
        public static readonly RoutedUICommand About = new("关于", nameof(About), typeof(HelpCommands));
        public static readonly RoutedUICommand CheckUpdates = new("检查更新", nameof(CheckUpdates), typeof(HelpCommands));
        public static readonly RoutedUICommand Feedback = new("反馈建议", nameof(Feedback), typeof(HelpCommands));
    }
}


