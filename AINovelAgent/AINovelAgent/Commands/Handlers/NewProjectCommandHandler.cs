using System;
using System.Threading;
using System.Threading.Tasks;
using AINovelAgent.Commands.Contracts;
using AINovelAgent.Commands.Requests;
using AINovelAgent.Services.Interfaces;
using AINovelAgent.Views.Windows;
using AINovelAgent.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace AINovelAgent.Commands.Handlers
{
    /// <summary>
    /// 新建项目命令处理器
    /// </summary>
    public sealed class NewProjectCommandHandler : ICommandHandler<NewProjectCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProjectBrowserService _projectBrowserService;

        public NewProjectCommandHandler(IServiceProvider serviceProvider, IProjectBrowserService projectBrowserService)
        {
            _serviceProvider = serviceProvider;
            _projectBrowserService = projectBrowserService;
        }

        public Task<CommandResult> HandleAsync(NewProjectCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                // 创建ViewModel和窗口
                var viewModel = _serviceProvider.GetRequiredService<CreateProjectViewModel>();
                var window = new CreateProjectWindow(viewModel);

                // 设置窗口的Owner（如果有主窗口的话）
                if (System.Windows.Application.Current.MainWindow != null)
                {
                    window.Owner = System.Windows.Application.Current.MainWindow;
                }

                // 显示模态对话框
                var result = window.ShowDialog();

                if (result == true)
                {
                    // 用户成功创建了项目，刷新项目浏览器
                    // 注意：这里可能需要获取创建的项目路径来加载
                    // 暂时返回成功，后续可以扩展为自动加载新创建的项目
                    return Task.FromResult(CommandResult.Success("项目创建成功"));
                }
                else
                {
                    return Task.FromResult(CommandResult.Success("用户取消了项目创建"));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(CommandResult.Failure($"创建项目失败：{ex.Message}"));
            }
        }
    }
}
