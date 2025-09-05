using System.Windows.Input;

namespace AINovelAgent.Commands.Definitions
{
    public static class ToolsCommands
    {
        public static readonly RoutedUICommand OpenSettings = new("设置", nameof(OpenSettings), typeof(ToolsCommands));
        public static readonly RoutedUICommand ManageAIModels = new("AI模型管理", nameof(ManageAIModels), typeof(ToolsCommands));
        public static readonly RoutedUICommand ManageKnowledgeBase = new("知识库管理", nameof(ManageKnowledgeBase), typeof(ToolsCommands));
        public static readonly RoutedUICommand ShowAnalysisReport = new("分析报告", nameof(ShowAnalysisReport), typeof(ToolsCommands));
    }
}


