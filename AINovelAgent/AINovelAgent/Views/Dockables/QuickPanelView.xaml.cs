using AINovelAgent.Base;
using AINovelAgent.ViewModels.Panels;
using Microsoft.Extensions.DependencyInjection;

namespace AINovelAgent.Views.Dockables
{
	public partial class QuickPanelView : DockableContentBase
	{
		public QuickPanelView()
		{
			InitializeComponent();
			Loaded += (s, e) =>
			{
				var vm = App.Services.GetRequiredService<ProjectNavigationViewModel>();
				DataContext = vm;
			};
		}
	}
}
