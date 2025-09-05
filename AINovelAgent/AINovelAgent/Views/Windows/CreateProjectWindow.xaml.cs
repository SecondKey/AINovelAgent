using System.Windows;
using AINovelAgent.ViewModels.Windows;

namespace AINovelAgent.Views.Windows
{
    /// <summary>
    /// CreateProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        public CreateProjectWindow(CreateProjectViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            // 订阅ViewModel的关闭事件
            viewModel.CloseRequested += (sender, result) =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}
