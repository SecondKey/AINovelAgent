using System;
using System.Threading;
using System.Threading.Tasks;
using AINovelAgent.Commands.Contracts;
using AINovelAgent.Commands.Requests;
using AINovelAgent.Services.Interfaces;

namespace AINovelAgent.Commands.Handlers
{
    public sealed class OpenProjectCommandHandler : ICommandHandler<OpenProjectCommand>
    {
        private readonly IDialogService _dialogService;
        private readonly IProjectBrowserService _projectBrowserService;
        private readonly IProjectValidationService _projectValidationService;
        private readonly ICurrentProjectService _currentProjectService;

        public OpenProjectCommandHandler(
            IDialogService dialogService, 
            IProjectBrowserService projectBrowserService,
            IProjectValidationService projectValidationService,
            ICurrentProjectService currentProjectService)
        {
            _dialogService = dialogService;
            _projectBrowserService = projectBrowserService;
            _projectValidationService = projectValidationService;
            _currentProjectService = currentProjectService;
        }

        public Task<CommandResult> HandleAsync(OpenProjectCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                // 显示文件夹选择对话框
                var folder = _dialogService.SelectFolder("选择 NovelAgent 工程文件夹");
                if (string.IsNullOrWhiteSpace(folder))
                {
                    return Task.FromResult(CommandResult.Failure("未选择工程文件夹"));
                }

                // 验证项目文件夹
                var validationResult = _projectValidationService.ValidateProject(folder);
                
                if (!validationResult.IsValid)
                {
                    // 显示验证错误信息
                    _dialogService.ShowError("无效的项目文件夹", validationResult.ErrorMessage);
                    return Task.FromResult(CommandResult.Failure($"项目验证失败：{validationResult.ErrorMessage}"));
                }

                // 如果有警告信息，显示警告对话框
                if (validationResult.Warnings.Count > 0)
                {
                    var warningMessage = "项目加载成功，但发现以下建议：\n\n" + string.Join("\n", validationResult.Warnings);
                    _dialogService.ShowWarning("项目建议", warningMessage);
                }

                // 设置当前工程
                var setResult = _currentProjectService.SetCurrentProject(folder);
                if (!setResult)
                {
                    return Task.FromResult(CommandResult.Failure("设置当前工程失败"));
                }

                // 加载项目到浏览器服务
                _projectBrowserService.LoadRoot(folder);

                var successMessage = !string.IsNullOrEmpty(validationResult.ProjectTitle) 
                    ? $"成功打开项目：{validationResult.ProjectTitle}"
                    : $"成功打开项目：{folder}";

                return Task.FromResult(CommandResult.Success(successMessage));
            }
            catch (Exception ex)
            {
                return Task.FromResult(CommandResult.Failure($"打开项目失败：{ex.Message}"));
            }
        }
    }
}


