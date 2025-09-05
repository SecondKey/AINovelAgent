using System;
using System.Collections.Generic;

namespace AINovelAgent.Services.Interfaces
{
    /// <summary>
    /// 项目验证结果
    /// </summary>
    public class ProjectValidationResult
    {
        /// <summary>
        /// 是否为有效的 NovelAgent 项目
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 验证错误信息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 警告信息列表
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// 项目标题（从 ProjectSettings.xml 中读取）
        /// </summary>
        public string? ProjectTitle { get; set; }

        /// <summary>
        /// 项目作者（从 ProjectSettings.xml 中读取）
        /// </summary>
        public string? ProjectAuthor { get; set; }
    }

    /// <summary>
    /// 项目验证服务接口
    /// </summary>
    public interface IProjectValidationService
    {
        /// <summary>
        /// 验证指定路径是否为有效的 NovelAgent 项目文件夹
        /// </summary>
        /// <param name="folderPath">要验证的文件夹路径</param>
        /// <returns>验证结果</returns>
        ProjectValidationResult ValidateProject(string folderPath);
    }
}
