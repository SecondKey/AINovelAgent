using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using AINovelAgent.Services.Interfaces;

namespace AINovelAgent.Services
{
    /// <summary>
    /// 当前工程服务实现
    /// </summary>
    public class CurrentProjectService : ICurrentProjectService
    {
        private CurrentProjectInfo? _currentProject;
        private readonly IProjectValidationService _validationService;

        public CurrentProjectService(IProjectValidationService validationService)
        {
            _validationService = validationService;
        }

        /// <summary>
        /// 当前工程信息
        /// </summary>
        public CurrentProjectInfo? CurrentProject
        {
            get => _currentProject;
            private set
            {
                if (_currentProject != value)
                {
                    _currentProject = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasCurrentProject));
                    CurrentProjectChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// 是否有打开的工程
        /// </summary>
        public bool HasCurrentProject => CurrentProject?.HasProject == true;

        /// <summary>
        /// 当前工程变更事件
        /// </summary>
        public event EventHandler<CurrentProjectInfo?>? CurrentProjectChanged;

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 设置当前工程
        /// </summary>
        /// <param name="projectPath">工程路径</param>
        /// <returns>是否设置成功</returns>
        public bool SetCurrentProject(string projectPath)
        {
            try
            {
                // 验证工程有效性
                var validationResult = _validationService.ValidateProject(projectPath);
                if (!validationResult.IsValid)
                {
                    return false;
                }

                // 读取工程信息
                var projectInfo = LoadProjectInfo(projectPath);
                if (projectInfo != null)
                {
                    CurrentProject = projectInfo;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭当前工程
        /// </summary>
        public void CloseCurrentProject()
        {
            CurrentProject = null;
        }

        /// <summary>
        /// 从工程路径加载工程信息
        /// </summary>
        /// <param name="projectPath">工程路径</param>
        /// <returns>工程信息</returns>
        private CurrentProjectInfo? LoadProjectInfo(string projectPath)
        {
            try
            {
                var projectSettingsPath = Path.Combine(projectPath, "ProjectSettings.xml");
                if (!File.Exists(projectSettingsPath))
                {
                    return null;
                }

                var doc = new XmlDocument();
                doc.Load(projectSettingsPath);

                var basicInfo = doc.SelectSingleNode("ProjectSettings/BasicInfo");
                if (basicInfo == null)
                {
                    return null;
                }

                var projectInfo = new CurrentProjectInfo
                {
                    ProjectPath = projectPath,
                    ProjectName = basicInfo.SelectSingleNode("Title")?.InnerText ?? Path.GetFileName(projectPath),
                    AuthorName = basicInfo.SelectSingleNode("Author")?.InnerText ?? string.Empty,
                    ProjectGenre = basicInfo.SelectSingleNode("Genre")?.InnerText ?? string.Empty
                };

                return projectInfo;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
