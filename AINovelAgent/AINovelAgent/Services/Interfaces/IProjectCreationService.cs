using System.Threading.Tasks;
using AINovelAgent.ViewModels.Windows;

namespace AINovelAgent.Services.Interfaces
{
    /// <summary>
    /// 项目创建结果
    /// </summary>
    public class ProjectCreationResult
    {
        /// <summary>
        /// 是否创建成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 创建的项目路径
        /// </summary>
        public string? ProjectPath { get; set; }

        /// <summary>
        /// 成功结果
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>成功结果</returns>
        public static ProjectCreationResult Success(string projectPath) => new()
        {
            IsSuccess = true,
            ProjectPath = projectPath
        };

        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>失败结果</returns>
        public static ProjectCreationResult Failure(string errorMessage) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    /// <summary>
    /// 项目创建服务接口
    /// </summary>
    public interface IProjectCreationService
    {
        /// <summary>
        /// 创建新的小说项目
        /// </summary>
        /// <param name="config">项目配置</param>
        /// <returns>创建结果</returns>
        Task<ProjectCreationResult> CreateProjectAsync(ProjectCreationConfig config);
    }
}
