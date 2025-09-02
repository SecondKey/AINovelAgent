using System.Windows;
using System.Windows.Controls;
using AINovelAgent.Utils.Services;
using AINovelAgent.Utils.Messaging;

namespace AINovelAgent.Views.Controls
{
	public partial class MainStatusBar : UserControl
	{
		public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register(
			name: nameof(StatusText),
			propertyType: typeof(string),
			ownerType: typeof(MainStatusBar),
			typeMetadata: new FrameworkPropertyMetadata("就绪"));

		public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(
			name: nameof(ProgressValue),
			propertyType: typeof(double),
			ownerType: typeof(MainStatusBar),
			typeMetadata: new FrameworkPropertyMetadata(0.0));

		public static readonly DependencyProperty IsProgressIndeterminateProperty = DependencyProperty.Register(
			name: nameof(IsProgressIndeterminate),
			propertyType: typeof(bool),
			ownerType: typeof(MainStatusBar),
			typeMetadata: new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty SystemStatusTextProperty = DependencyProperty.Register(
			name: nameof(SystemStatusText),
			propertyType: typeof(string),
			ownerType: typeof(MainStatusBar),
			typeMetadata: new FrameworkPropertyMetadata("Idle"));

		public string StatusText
		{
			get => (string)GetValue(StatusTextProperty);
			set => SetValue(StatusTextProperty, value);
		}

		public double ProgressValue
		{
			get => (double)GetValue(ProgressValueProperty);
			set => SetValue(ProgressValueProperty, value);
		}

		public bool IsProgressIndeterminate
		{
			get => (bool)GetValue(IsProgressIndeterminateProperty);
			set => SetValue(IsProgressIndeterminateProperty, value);
		}

		public string SystemStatusText
		{
			get => (string)GetValue(SystemStatusTextProperty);
			set => SetValue(SystemStatusTextProperty, value);
		}

		public MainStatusBar()
		{
			InitializeComponent();
			StatusBarService.StatusTextChanged += OnStatusTextChanged;
			StatusText = StatusBarService.StatusText;
			// 通用通知订阅
			MessengerHub.Subscribe(this, "Status/TextUpdated", payload =>
			{
				if (payload is string text)
				{
					Dispatcher.Invoke(() => StatusText = text);
				}
			});
			MessengerHub.Subscribe(this, "Status/Progress", payload =>
			{
				if (payload is double value)
				{
					Dispatcher.Invoke(() => { IsProgressIndeterminate = false; ProgressValue = value; });
				}
				else if (payload is bool indeterminate)
				{
					Dispatcher.Invoke(() => { IsProgressIndeterminate = indeterminate; });
				}
			});
			MessengerHub.Subscribe(this, "Status/System", payload =>
			{
				if (payload is string text)
				{
					Dispatcher.Invoke(() => SystemStatusText = text);
				}
			});
		}

		private void OnStatusTextChanged(string text)
		{
			Dispatcher.Invoke(() => StatusText = text);
		}
	}
}
