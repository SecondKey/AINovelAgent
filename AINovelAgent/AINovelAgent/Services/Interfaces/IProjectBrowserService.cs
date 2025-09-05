using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace AINovelAgent.Services.Interfaces
{
    public interface IProjectBrowserService
    {
        ReadOnlyObservableCollection<ProjectItem> RootItems { get; }
        string? CurrentRoot { get; }
        void LoadRoot(string rootDirectory);
    }

    public sealed class ProjectItem : INotifyPropertyChanged
    {
        public required string Name { get; init; }
        public required string FullPath { get; init; }
        public required bool IsDirectory { get; init; }
        
        private ObservableCollection<ProjectItem>? _children;
        private bool _isExpanded;
        private bool _hasLoadedChildren;

        public ObservableCollection<ProjectItem> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<ProjectItem>();
                    if (IsDirectory && !_hasLoadedChildren)
                    {
                        LoadChildren();
                    }
                }
                return _children;
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
                    if (value && !_hasLoadedChildren)
                    {
                        LoadChildren();
                    }
                }
            }
        }

        public bool HasChildren => IsDirectory && (Children.Any() || !_hasLoadedChildren);

        public event PropertyChangedEventHandler? PropertyChanged;

        private void LoadChildren()
        {
            if (!IsDirectory || _hasLoadedChildren) return;
            
            try
            {
                if (Directory.Exists(FullPath))
                {
                    // 加载子目录
                    foreach (var dir in Directory.GetDirectories(FullPath))
                    {
                        Children.Add(new ProjectItem
                        {
                            Name = Path.GetFileName(dir),
                            FullPath = dir,
                            IsDirectory = true
                        });
                    }

                    // 加载文件
                    foreach (var file in Directory.GetFiles(FullPath))
                    {
                        Children.Add(new ProjectItem
                        {
                            Name = Path.GetFileName(file),
                            FullPath = file,
                            IsDirectory = false
                        });
                    }
                }
                _hasLoadedChildren = true;
            }
            catch
            {
                // 忽略权限错误等异常
            }
        }
    }
}


