namespace AINovelAgent.Utils.Messaging.Messages
{
	public sealed class StatusTextMessage
	{
		public string Text { get; }
		public StatusTextMessage(string text) => Text = text ?? string.Empty;
	}
}
