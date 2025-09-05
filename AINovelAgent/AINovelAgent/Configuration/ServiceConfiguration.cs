using AINovelAgent.Services;
using AINovelAgent.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AINovelAgent.Commands.Bus;
using AINovelAgent.Commands.Contracts;
using AINovelAgent.Commands.Requests;
using AINovelAgent.Commands.Handlers;
using AINovelAgent.ViewModels.Windows;

namespace AINovelAgent.Configuration
{
    /// <summary>
    /// 服务配置类
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>配置后的服务集合</returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // 注册服务接口和实现
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IEditorService, EditorService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IDockingLayoutService, AINovelAgent.Services.UI.DockingLayoutService>();
            services.AddSingleton<IProjectBrowserService, ProjectBrowserService>();
            services.AddSingleton<IProjectValidationService, ProjectValidationService>();
            services.AddSingleton<IProjectCreationService, ProjectCreationService>();
            services.AddSingleton<ICurrentProjectService, CurrentProjectService>();

            // 命令总线
            services.AddSingleton<ICommandBus, CommandBus>();

            // 命令处理器
            services.AddTransient<ICommandHandler<OpenProjectCommand>, OpenProjectCommandHandler>();
            services.AddTransient<ICommandHandler<NewProjectCommand>, NewProjectCommandHandler>();

            // 注册ViewModels
            services.AddTransient<ViewModels.MainEditorViewModel>();
            services.AddTransient<ViewModels.Panels.ProjectNavigationViewModel>();
            services.AddTransient<CreateProjectViewModel>();
            services.AddTransient<ViewModels.MainWindowViewModel>();

            // 注册Windows
            services.AddTransient<MainWindow>();
            services.AddTransient<Views.Dockables.MainEditorView>();

            // 注册日志服务
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            return services;
        }

        /// <summary>
        /// 创建主机构建器
        /// </summary>
        /// <returns>主机构建器</returns>
        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfigureServices(services));
        }
    }
}
