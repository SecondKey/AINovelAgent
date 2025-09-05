using AINovelAgent.Services.Interfaces;
using Microsoft.Win32;
using System.Windows;

namespace AINovelAgent.Services
{
    /// <summary>
    /// 对话框服务实现
    /// </summary>
    public class DialogService : IDialogService
    {
        public string? ShowSaveFileDialog(string filter, string defaultExt)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt
            };
            
            return saveDialog.ShowDialog() == true ? saveDialog.FileName : null;
        }

        public string? ShowOpenFileDialog(string filter)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = filter
            };
            
            return openDialog.ShowDialog() == true ? openDialog.FileName : null;
        }

        public MessageBoxResult ShowMessageBox(string message, string title, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            return MessageBox.Show(message, title, button, icon);
        }

        public string? SelectFolder(string? description = null)
        {
            var dlg = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = false,
                EnsurePathExists = true,
                Title = description ?? "选择工程文件夹"
            };
            var result = dlg.ShowDialog();
            return result == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok ? dlg.FileName : null;
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarning(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
