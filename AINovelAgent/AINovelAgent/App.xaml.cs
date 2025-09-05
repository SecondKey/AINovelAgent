using AINovelAgent.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AINovelAgent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;
        public static IHost? HostInstance { get; private set; }
        public static IServiceProvider Services => HostInstance!.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            // 创建主机
            _host = ServiceConfiguration.CreateHostBuilder().Build();
            HostInstance = _host;

            // 启动主机
            await _host.StartAsync();

            // 获取主窗口并显示
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }
}
