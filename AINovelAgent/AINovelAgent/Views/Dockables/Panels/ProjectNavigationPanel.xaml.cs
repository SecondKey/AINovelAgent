using System.Windows.Controls;
using AINovelAgent.Services.Interfaces;

namespace AINovelAgent.Views.Dockables.Panels
{
    public partial class ProjectNavigationPanel : UserControl
    {
        public ProjectNavigationPanel()
        {
            InitializeComponent();
            // DataContext 将在窗口/DI 组合根中设置，这里不直接解析服务
        }
    }
}
