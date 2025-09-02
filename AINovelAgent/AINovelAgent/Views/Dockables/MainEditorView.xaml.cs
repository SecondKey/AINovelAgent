using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using AINovelAgent.Base;

namespace AINovelAgent.Views.Dockables
{
	public partial class MainEditorView : DockableContentBase
	{
		public MainEditorView()
		{
			InitializeComponent();
		}

		private void EditorBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateStatus();
		}

		private void EditorBox_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
		{
			UpdateStatus();
		}

		private void EditorBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Tab)
			{
				// 插入制表符，阻止焦点跳转
				var box = EditorBox;
				int caret = box.CaretIndex;
				box.Text = box.Text.Insert(caret, "\t");
				box.CaretIndex = caret + 1;
				e.Handled = true;
			}
		}

		private void UpdateStatus()
		{
			if (EditorBox == null) return;
			var text = EditorBox.Text ?? string.Empty;
			var caretIndex = EditorBox.CaretIndex;
			// 计算行列
			var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
			int currentLine = 1;
			int column = 1;
			int count = 0;
			for (int i = 0; i < lines.Length; i++)
			{
				var lineLen = lines[i].Length + (i < lines.Length - 1 ? 1 : 0); // 近似处理换行
				if (count + lineLen >= caretIndex)
				{
					currentLine = i + 1;
					column = Math.Max(1, caretIndex - count + 1);
					break;
				}
				count += lineLen;
			}
			var charCount = text.Length;
			if (EditorStatusTextBlock != null)
			{
				EditorStatusTextBlock.Text = $"行 {currentLine}, 列 {column}    字数 {charCount}";
			}
		}
	}
}
