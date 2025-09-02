using System.Windows.Input;

namespace AINovelAgent.Views.Controls
{
	public static class MenuCommands
	{
		public static readonly RoutedUICommand SaveLayout = new RoutedUICommand(
			"保存布局", nameof(SaveLayout), typeof(MenuCommands));

		public static readonly RoutedUICommand LoadLayout = new RoutedUICommand(
			"加载布局", nameof(LoadLayout), typeof(MenuCommands));

		public static readonly RoutedUICommand ResetLayout = new RoutedUICommand(
			"重置布局", nameof(ResetLayout), typeof(MenuCommands));
	}
}
