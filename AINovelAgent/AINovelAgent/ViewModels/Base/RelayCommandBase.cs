using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AINovelAgent.ViewModels.Base
{
    /// <summary>
    /// 自定义RelayCommand基类，提供额外的功能
    /// </summary>
    public abstract class RelayCommandBase : ICommand
    {
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        protected RelayCommandBase(Func<bool>? canExecute = null)
        {
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public abstract void Execute(object? parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 同步RelayCommand
    /// </summary>
    public class SyncRelayCommand : RelayCommandBase
    {
        private readonly Action _execute;

        public SyncRelayCommand(Action execute, Func<bool>? canExecute = null) 
            : base(canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public override void Execute(object? parameter)
        {
            _execute();
        }
    }

    /// <summary>
    /// 异步RelayCommand
    /// </summary>
    public class AsyncRelayCommand : RelayCommandBase
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;

        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null) 
            : base(canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override async void Execute(object? parameter)
        {
            await _execute();
        }
    }

    /// <summary>
    /// 带参数的异步RelayCommand
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class AsyncRelayCommand<T> : RelayCommandBase
    {
        private readonly Func<T?, Task> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public AsyncRelayCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null) 
            : base(() => canExecute?.Invoke(default(T)) ?? true)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override async void Execute(object? parameter)
        {
            await _execute((T?)parameter);
        }

        public new bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke((T?)parameter) ?? true;
        }
    }
}
