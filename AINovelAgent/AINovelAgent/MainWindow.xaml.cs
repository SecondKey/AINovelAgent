using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AINovelAgent.Services.Interfaces;
using AINovelAgent.Services.UI;
using Microsoft.Extensions.DependencyInjection;
using AINovelAgent.Commands.Routing;
using AINovelAgent.Commands.Bus;
using AINovelAgent.Commands.Definitions;
using AINovelAgent.ViewModels.Panels;
using AINovelAgent.ViewModels;
using AINovelAgent.Views.Dockables.Panels;

namespace AINovelAgent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IServiceProvider serviceProvider) : this()
        {
            // 设置主窗口ViewModel
            var mainWindowVm = serviceProvider.GetRequiredService<MainWindowViewModel>();
            DataContext = mainWindowVm;

            // 命令绑定
            var bus = serviceProvider.GetRequiredService<ICommandBus>();
            var router = new CommandRouter(bus);
            CommandBindingsProvider.Attach(this, router,
                FileCommands.New, FileCommands.Open, FileCommands.Save, FileCommands.SaveAs, FileCommands.Exit,
                EditCommands.Undo, EditCommands.Redo, EditCommands.Cut, EditCommands.Copy, EditCommands.Paste, EditCommands.Find, EditCommands.Replace,
                ViewCommands.ToggleSidebar, ViewCommands.ToggleToolbar, ViewCommands.ToggleStatusBar, ViewCommands.Fullscreen
            );

            // 左侧目录视图模型绑定
            var projectVm = serviceProvider.GetRequiredService<ProjectNavigationViewModel>();
            var projectPanel = FindNameRecursive<ProjectNavigationPanel>(this);
            if (projectPanel != null)
            {
                projectPanel.DataContext = projectVm;
            }
        }

        private static T? FindNameRecursive<T>(DependencyObject root) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child is T t) return t;
                var sub = FindNameRecursive<T>(child);
                if (sub != null) return sub;
            }
            return null;
        }
    }
}