using Microsoft.Win32;
using System.Windows;

namespace AINovelAgent.Services.Interfaces
{
    /// <summary>
    /// 对话框服务接口
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// 显示保存文件对话框
        /// </summary>
        /// <param name="filter">文件过滤器</param>
        /// <param name="defaultExt">默认扩展名</param>
        /// <returns>选择的文件路径，如果取消则返回null</returns>
        string? ShowSaveFileDialog(string filter, string defaultExt);

        /// <summary>
        /// 显示打开文件对话框
        /// </summary>
        /// <param name="filter">文件过滤器</param>
        /// <returns>选择的文件路径，如果取消则返回null</returns>
        string? ShowOpenFileDialog(string filter);

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="button">按钮类型</param>
        /// <param name="icon">图标类型</param>
        /// <returns>用户选择的结果</returns>
        MessageBoxResult ShowMessageBox(string message, string title, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information);

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="description">对话框描述</param>
        /// <returns>选择的文件夹路径，否则为 null</returns>
        string? SelectFolder(string? description = null);

        /// <summary>
        /// 显示错误对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">错误消息</param>
        void ShowError(string title, string message);

        /// <summary>
        /// 显示警告对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">警告消息</param>
        void ShowWarning(string title, string message);
    }
}
