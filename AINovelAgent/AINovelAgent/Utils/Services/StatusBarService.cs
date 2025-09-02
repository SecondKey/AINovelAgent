using System;

namespace AINovelAgent.Utils.Services
{
	public static class StatusBarService
	{
		private static string _statusText = "就绪";
		public static event Action<string>? StatusTextChanged;

		public static string StatusText
		{
			get => _statusText;
			set
			{
				_statusText = value ?? string.Empty;
				StatusTextChanged?.Invoke(_statusText);
			}
		}

		public static void SetText(string text)
		{
			StatusText = text;
		}
	}
}
