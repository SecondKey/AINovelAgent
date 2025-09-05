using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AINovelAgent.Services.Interfaces;
using AINovelAgent.ViewModels.Base;

namespace AINovelAgent.ViewModels.Windows
{
    /// <summary>
    /// 创建项目窗口的ViewModel
    /// </summary>
    public class CreateProjectViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IProjectCreationService _projectCreationService;
        private readonly ICurrentProjectService _currentProjectService;
        private readonly IProjectBrowserService _projectBrowserService;

        #region 属性

        private string _projectPath = string.Empty;
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (SetProperty(ref _projectPath, value))
                {
                    OnPropertyChanged(nameof(ProjectFullPath));
                    OnPropertyChanged(nameof(CanCreateProject));
                    CreateProjectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _projectName = string.Empty;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (SetProperty(ref _projectName, value))
                {
                    OnPropertyChanged(nameof(ProjectFullPath));
                    OnPropertyChanged(nameof(CanCreateProject));
                    CreateProjectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // 移除NovelTitle字段，项目名称即为主要标识

        private string _authorName = string.Empty;
        public string AuthorName
        {
            get => _authorName;
            set
            {
                SetProperty(ref _authorName, value);
                OnPropertyChanged(nameof(CanCreateProject));
                CreateProjectCommand.RaiseCanExecuteChanged();
            }
        }

        private string _projectGenre = "通用项目";
        public string ProjectGenre
        {
            get => _projectGenre;
            set => SetProperty(ref _projectGenre, value);
        }

        private int _targetWordCount = 100000;
        public int TargetWordCount
        {
            get => _targetWordCount;
            set => SetProperty(ref _targetWordCount, value);
        }

        private int _chapterCount = 20;
        public int ChapterCount
        {
            get => _chapterCount;
            set => SetProperty(ref _chapterCount, value);
        }

        private string _writingStyle = "王道热血";
        public string WritingStyle
        {
            get => _writingStyle;
            set => SetProperty(ref _writingStyle, value);
        }

        private string _narrativePerspective = "第三人称";
        public string NarrativePerspective
        {
            get => _narrativePerspective;
            set => SetProperty(ref _narrativePerspective, value);
        }

        private string _projectDescription = string.Empty;
        public string ProjectDescription
        {
            get => _projectDescription;
            set => SetProperty(ref _projectDescription, value);
        }

        /// <summary>
        /// 项目完整路径（只读）
        /// </summary>
        public string ProjectFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ProjectPath) || string.IsNullOrWhiteSpace(ProjectName))
                    return "请选择项目路径和输入项目名称";
                
                return Path.Combine(ProjectPath, ProjectName);
            }
        }

        /// <summary>
        /// 是否可以创建项目
        /// </summary>
        public bool CanCreateProject
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProjectPath) &&
                       !string.IsNullOrWhiteSpace(ProjectName) &&
                       !string.IsNullOrWhiteSpace(AuthorName);
            }
        }

        #endregion

        #region 命令

        public ICommand BrowseProjectPathCommand { get; }
        public SyncRelayCommand CreateProjectCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion

        #region 事件

        /// <summary>
        /// 请求关闭窗口事件
        /// </summary>
        public event EventHandler<bool>? CloseRequested;

        #endregion

        public CreateProjectViewModel(
            IDialogService dialogService, 
            IProjectCreationService projectCreationService,
            ICurrentProjectService currentProjectService,
            IProjectBrowserService projectBrowserService)
        {
            _dialogService = dialogService;
            _projectCreationService = projectCreationService;
            _currentProjectService = currentProjectService;
            _projectBrowserService = projectBrowserService;

            BrowseProjectPathCommand = new SyncRelayCommand(BrowseProjectPath);
            CreateProjectCommand = new SyncRelayCommand(CreateProject, () => CanCreateProject);
            CancelCommand = new SyncRelayCommand(Cancel);

            // 设置默认作者名称（可以从配置中读取）
            AuthorName = Environment.UserName;
        }

        #region 命令处理方法

        private void BrowseProjectPath()
        {
            var selectedPath = _dialogService.SelectFolder("选择项目存放路径");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                ProjectPath = selectedPath;
            }
        }

        private async void CreateProject()
        {
            try
            {
                // 检查项目路径是否已存在
                var fullPath = ProjectFullPath;
                if (Directory.Exists(fullPath))
                {
                    var result = _dialogService.ShowMessageBox(
                        $"目录 '{fullPath}' 已存在。是否要覆盖？",
                        "确认覆盖",
                        System.Windows.MessageBoxButton.YesNo,
                        System.Windows.MessageBoxImage.Warning);

                    if (result != System.Windows.MessageBoxResult.Yes)
                        return;
                }

                // 创建项目配置
                var projectConfig = new ProjectCreationConfig
                {
                    ProjectPath = fullPath,
                    ProjectName = ProjectName,
                    AuthorName = AuthorName,
                    ProjectGenre = ProjectGenre,
                    TargetWordCount = TargetWordCount,
                    ChapterCount = ChapterCount,
                    WritingStyle = WritingStyle,
                    NarrativePerspective = NarrativePerspective,
                    ProjectDescription = ProjectDescription
                };

                // 创建项目
                var creationResult = await _projectCreationService.CreateProjectAsync(projectConfig);
                
                if (creationResult.IsSuccess)
                {
                    // 设置当前工程
                    var setResult = _currentProjectService.SetCurrentProject(fullPath);
                    if (setResult)
                    {
                        // 加载项目到浏览器服务
                        _projectBrowserService.LoadRoot(fullPath);
                        
                        // 关闭窗口并返回成功（不显示成功对话框）
                        CloseRequested?.Invoke(this, true);
                    }
                    else
                    {
                        _dialogService.ShowError("打开失败", "项目创建成功，但无法自动打开，请手动打开该项目");
                        CloseRequested?.Invoke(this, true);
                    }
                }
                else
                {
                    _dialogService.ShowError("创建失败", creationResult.ErrorMessage ?? "未知错误");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError("创建失败", $"创建项目时发生错误：{ex.Message}");
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        #endregion
    }

    /// <summary>
    /// 项目创建配置
    /// </summary>
    public class ProjectCreationConfig
    {
        public string ProjectPath { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string ProjectGenre { get; set; } = string.Empty;
        public int TargetWordCount { get; set; }
        public int ChapterCount { get; set; }
        public string WritingStyle { get; set; } = string.Empty;
        public string NarrativePerspective { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
    }
}
