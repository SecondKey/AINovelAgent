using System.ComponentModel;
using AINovelAgent.Services.Interfaces;
using AINovelAgent.ViewModels.Base;

namespace AINovelAgent.ViewModels
{
    /// <summary>
    /// 主窗口ViewModel
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ICurrentProjectService _currentProjectService;
        private string _windowTitle = "AINovelAgent";

        public MainWindowViewModel(ICurrentProjectService currentProjectService)
        {
            _currentProjectService = currentProjectService;
            
            // 监听当前工程变更
            _currentProjectService.CurrentProjectChanged += OnCurrentProjectChanged;
            _currentProjectService.PropertyChanged += OnCurrentProjectServicePropertyChanged;
            
            // 初始化窗口标题
            UpdateWindowTitle();
        }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string WindowTitle
        {
            get => _windowTitle;
            private set => SetProperty(ref _windowTitle, value);
        }

        /// <summary>
        /// 当前工程变更事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="project">工程信息</param>
        private void OnCurrentProjectChanged(object? sender, CurrentProjectInfo? project)
        {
            UpdateWindowTitle();
        }

        /// <summary>
        /// 当前工程服务属性变更事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnCurrentProjectServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ICurrentProjectService.CurrentProject) || 
                e.PropertyName == nameof(ICurrentProjectService.HasCurrentProject))
            {
                UpdateWindowTitle();
            }
        }

        /// <summary>
        /// 更新窗口标题
        /// </summary>
        private void UpdateWindowTitle()
        {
            if (_currentProjectService.HasCurrentProject && _currentProjectService.CurrentProject != null)
            {
                var project = _currentProjectService.CurrentProject;
                WindowTitle = $"AINovelAgent - {project.ProjectName}";
            }
            else
            {
                WindowTitle = "AINovelAgent";
            }
        }
    }
}
