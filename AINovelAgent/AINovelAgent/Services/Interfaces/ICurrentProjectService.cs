using System;
using System.ComponentModel;

namespace AINovelAgent.Services.Interfaces
{
    /// <summary>
    /// 当前工程信息
    /// </summary>
    public class CurrentProjectInfo
    {
        /// <summary>
        /// 工程根路径
        /// </summary>
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 工程名称
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// 作者名称
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// 工程类型
        /// </summary>
        public string ProjectGenre { get; set; } = string.Empty;

        /// <summary>
        /// 是否有打开的工程
        /// </summary>
        public bool HasProject => !string.IsNullOrWhiteSpace(ProjectPath);
    }

    /// <summary>
    /// 当前工程服务接口
    /// </summary>
    public interface ICurrentProjectService : INotifyPropertyChanged
    {
        /// <summary>
        /// 当前工程信息
        /// </summary>
        CurrentProjectInfo? CurrentProject { get; }

        /// <summary>
        /// 是否有打开的工程
        /// </summary>
        bool HasCurrentProject { get; }

        /// <summary>
        /// 设置当前工程
        /// </summary>
        /// <param name="projectPath">工程路径</param>
        /// <returns>是否设置成功</returns>
        bool SetCurrentProject(string projectPath);

        /// <summary>
        /// 关闭当前工程
        /// </summary>
        void CloseCurrentProject();

        /// <summary>
        /// 当前工程变更事件
        /// </summary>
        event EventHandler<CurrentProjectInfo?>? CurrentProjectChanged;
    }
}
