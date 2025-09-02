using System.Threading.Tasks;
using AvalonDock;

namespace AINovelAgent.Services.Interfaces
{
	public interface IDockingLayoutService
	{
		Task LoadLayoutAsync(DockingManager dockingManager);
		Task SaveLayoutAsync(DockingManager dockingManager);
		Task ResetLayoutAsync(DockingManager dockingManager);
		string GetDefaultLayoutXml();
	}
}
