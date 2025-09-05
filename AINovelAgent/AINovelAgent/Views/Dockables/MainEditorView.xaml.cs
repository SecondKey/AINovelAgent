using AINovelAgent.Base;
using AINovelAgent.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AINovelAgent.Views.Dockables
{
    /// <summary>
    /// MainEditorView.xaml 的交互逻辑
    /// </summary>
	public partial class MainEditorView : DockableContentBase
	{
        private MainEditorViewModel? _viewModel;

		public MainEditorView()
		{
			InitializeComponent();
            Loaded += OnLoaded;
        }

        public MainEditorView(IServiceProvider serviceProvider) : this()
        {
            // 通过DI获取ViewModel
            _viewModel = serviceProvider.GetRequiredService<MainEditorViewModel>();
            DataContext = _viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 如果还没有设置ViewModel，尝试从DataContext获取
            if (_viewModel == null)
            {
                _viewModel = DataContext as MainEditorViewModel;
            }

            // 设置Document属性
            if (_viewModel != null)
            {
                EditorBox.Document = _viewModel.Document;
            }
		}

		private void EditorBox_TextChanged(object sender, TextChangedEventArgs e)
		{
            // 同步Document到ViewModel
            if (_viewModel != null && EditorBox.Document != _viewModel.Document)
            {
                _viewModel.Document = EditorBox.Document;
            }
            _viewModel?.UpdateStatusCommand.Execute(null);
		}

		private void EditorBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
            _viewModel?.UpdateStatusCommand.Execute(null);
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null && e.AddedItems.Count > 0)
            {
                _viewModel.SelectedFontFamily = e.AddedItems[0].ToString() ?? "Microsoft YaHei";
                _viewModel.ApplyFontFamilyCommand.Execute(null);
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null && e.AddedItems.Count > 0)
            {
                if (double.TryParse(e.AddedItems[0].ToString(), out double fontSize))
                {
                    _viewModel.SelectedFontSize = fontSize;
                    _viewModel.ApplyFontSizeCommand.Execute(null);
                }
            }
        }

		private void LineSpacingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (_viewModel != null && e.AddedItems.Count > 0)
			{
                if (double.TryParse(e.AddedItems[0].ToString(), out double lineSpacing))
				{
                    _viewModel.SelectedLineSpacing = lineSpacing;
                    _viewModel.ApplyLineSpacingCommand.Execute(null);
				}
			}
		}

		private void CharacterSpacingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (_viewModel != null && e.AddedItems.Count > 0)
            {
                if (double.TryParse(e.AddedItems[0].ToString(), out double characterSpacing))
                {
                    _viewModel.SelectedCharacterSpacing = characterSpacing;
                    _viewModel.ApplyCharacterSpacingCommand.Execute(null);
                }
            }
        }
    }
}