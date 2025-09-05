using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace AINovelAgent.ViewModels.Base
{
    /// <summary>
    /// ViewModel基类，提供通用的属性和命令功能
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _title = string.Empty;

        /// <summary>
        /// 获取或设置是否正在执行操作
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// 执行操作时设置忙碌状态
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <returns></returns>
        protected async Task ExecuteAsync(Func<Task> action)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await action();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 执行操作时设置忙碌状态
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="action">要执行的操作</param>
        /// <returns></returns>
        protected async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            if (IsBusy) return default(T);

            try
            {
                IsBusy = true;
                return await action();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
