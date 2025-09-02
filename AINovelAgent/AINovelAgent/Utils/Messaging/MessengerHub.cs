using System;
using CommunityToolkit.Mvvm.Messaging;

namespace AINovelAgent.Utils.Messaging
{
	public static class MessengerHub
	{
		private static readonly IMessenger _messenger = WeakReferenceMessenger.Default;

		// 订阅指定主题（使用 token 通道避免重复订阅冲突）
		public static void Subscribe(object recipient, string topic, Action<object?> handler)
		{
			_messenger.Register<NotificationMessage, string>(recipient, topic, (r, m) =>
			{
				handler?.Invoke(m.Payload);
			});
		}

		// 按主题注销
		public static void Unsubscribe(object recipient, string topic)
		{
			_messenger.Unregister<NotificationMessage, string>(recipient, topic);
		}

		public static void UnsubscribeAll(object recipient)
		{
			_messenger.UnregisterAll(recipient);
		}

		// 发布通知（topic + payload）
		public static void Publish(string topic, object? payload = null)
		{
			_messenger.Send<NotificationMessage, string>(new NotificationMessage(topic, payload), topic);
		}
	}

	public sealed class NotificationMessage
	{
		public string Topic { get; }
		public object? Payload { get; }
		public NotificationMessage(string topic, object? payload)
		{
			Topic = topic ?? string.Empty;
			Payload = payload;
		}
	}
}
