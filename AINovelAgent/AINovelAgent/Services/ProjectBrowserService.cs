using System.Collections.ObjectModel;
using System.IO;
using AINovelAgent.Services.Interfaces;

namespace AINovelAgent.Services
{
    public sealed class ProjectBrowserService : IProjectBrowserService
    {
        private readonly ObservableCollection<ProjectItem> _rootItems = new();
        private readonly ReadOnlyObservableCollection<ProjectItem> _roRootItems;
        public string? CurrentRoot { get; private set; }

        public ProjectBrowserService()
        {
            _roRootItems = new ReadOnlyObservableCollection<ProjectItem>(_rootItems);
        }

        public ReadOnlyObservableCollection<ProjectItem> RootItems => _roRootItems;

        public void LoadRoot(string rootDirectory)
        {
            CurrentRoot = rootDirectory;
            _rootItems.Clear();

            if (!Directory.Exists(rootDirectory)) return;

            // 只加载根目录的顶层项目，子项目将按需加载
            foreach (var dir in Directory.GetDirectories(rootDirectory))
            {
                _rootItems.Add(new ProjectItem
                {
                    Name = Path.GetFileName(dir),
                    FullPath = dir,
                    IsDirectory = true
                });
            }

            foreach (var file in Directory.GetFiles(rootDirectory))
            {
                _rootItems.Add(new ProjectItem
                {
                    Name = Path.GetFileName(file),
                    FullPath = file,
                    IsDirectory = false
                });
            }
        }
    }
}


