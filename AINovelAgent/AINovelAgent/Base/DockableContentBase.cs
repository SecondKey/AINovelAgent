using System.Windows;
using System.Windows.Controls;

namespace AINovelAgent.Base
{
	public abstract class DockableContentBase : UserControl
	{
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
			name: nameof(Title),
			propertyType: typeof(string),
			ownerType: typeof(DockableContentBase),
			typeMetadata: new FrameworkPropertyMetadata(default(string)));

		public string? Title
		{
			get => (string?)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		public static readonly DependencyProperty ContentIdProperty = DependencyProperty.Register(
			name: nameof(ContentId),
			propertyType: typeof(string),
			ownerType: typeof(DockableContentBase),
			typeMetadata: new FrameworkPropertyMetadata(default(string)));

		public string? ContentId
		{
			get => (string?)GetValue(ContentIdProperty);
			set => SetValue(ContentIdProperty, value);
		}
	}
}
