using System.Collections.ObjectModel;
using AINovelAgent.Services.Interfaces;

namespace AINovelAgent.ViewModels.Panels
{
    public sealed class ProjectNavigationViewModel
    {
        private readonly IProjectBrowserService _browserService;
        public ReadOnlyObservableCollection<ProjectItem> RootItems => _browserService.RootItems;
        public string? CurrentRoot => _browserService.CurrentRoot;

        public ProjectNavigationViewModel(IProjectBrowserService browserService)
        {
            _browserService = browserService;
        }
    }
}


