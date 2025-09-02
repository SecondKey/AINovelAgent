using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AINovelAgent.Base;
using AINovelAgent.Utils.Services;

namespace AINovelAgent.Views.Dockables
{
	public partial class AIChatView : DockableContentBase
	{
		private readonly ObservableCollection<string> _messages = new ObservableCollection<string>();

		public AIChatView()
		{
			InitializeComponent();
			MessagesList.ItemsSource = _messages;
		}

		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			SendCurrentInput();
		}

		private void InputBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) == 0)
			{
				e.Handled = true;
				SendCurrentInput();
			}
		}

		private void SendCurrentInput()
		{
			var text = (InputBox.Text ?? string.Empty).Trim();
			if (string.IsNullOrEmpty(text)) return;

			_messages.Add($"你: {text}");
			InputBox.Clear();
			MessagesList.ScrollIntoView(_messages[^1]);
			StatusBarService.SetText("发送中...");

			// 占位：模拟AI回复
			_messages.Add("AI: （占位回复）此处将显示AI响应内容。");
			MessagesList.ScrollIntoView(_messages[^1]);
			StatusBarService.SetText("就绪");
		}
	}
}
