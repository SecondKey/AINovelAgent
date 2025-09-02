namespace AINovelAgent.Utils.Messaging.Messages
{
	public sealed class ChatMessage
	{
		public string Role { get; }
		public string Content { get; }
		public ChatMessage(string role, string content)
		{
			Role = role ?? string.Empty;
			Content = content ?? string.Empty;
		}
	}
}
