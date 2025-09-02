using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Text;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using AINovelAgent.Services.Interfaces;
using AINovelAgent.Views.Dockables;

namespace AINovelAgent.Services.UI
{
	public sealed class DockingLayoutService : IDockingLayoutService
	{
		private readonly string _layoutFilePath;
		private const string NsHttp = "http://schemas.xceed.com/wpf/xaml/avalondock/layout";
		private const string NsUrn = "urn:avalondock-layout";

		public DockingLayoutService()
		{
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var appDir = Path.Combine(appData, "AINovelAgent");
			Directory.CreateDirectory(appDir);
			_layoutFilePath = Path.Combine(appDir, "layout.xml");
		}

		public async Task LoadLayoutAsync(DockingManager dockingManager)
		{
			try
			{
				if (File.Exists(_layoutFilePath))
				{
					var xml = await File.ReadAllTextAsync(_layoutFilePath, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
					xml = PreprocessXml(xml);
					var serializer = CreateSerializerWithCallback(dockingManager);
					using var sr = new StringReader(xml);
					serializer.Deserialize(sr);
				}
				else
				{
					await ResetLayoutAsync(dockingManager);
				}
			}
			catch (Exception)
			{
				await ResetLayoutAsync(dockingManager);
			}
		}

		public async Task SaveLayoutAsync(DockingManager dockingManager)
		{
			using var ms = new MemoryStream();
			var serializer = new XmlLayoutSerializer(dockingManager);
			serializer.Serialize(ms);
			ms.Position = 0;
			using var sr = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
			var xml = await sr.ReadToEndAsync();
			xml = TrimBom(xml);
			await File.WriteAllTextAsync(_layoutFilePath, xml, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}

		public Task ResetLayoutAsync(DockingManager dockingManager)
		{
			var serializer = CreateSerializerWithCallback(dockingManager);
			var exeDir = AppDomain.CurrentDomain.BaseDirectory;
			var defaultLayoutPath = Path.Combine(exeDir, "Resources", "Layouts", "DefaultLayout.xml");
			if (File.Exists(defaultLayoutPath))
			{
				var xml = File.ReadAllText(defaultLayoutPath, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
				xml = PreprocessXml(xml);
				using var sr = new StringReader(xml);
				serializer.Deserialize(sr);
			}
			else
			{
				var defaultXml = GetDefaultLayoutXml();
				defaultXml = PreprocessXml(defaultXml);
				using var stringReader = new StringReader(defaultXml);
				serializer.Deserialize(stringReader);
			}
			return Task.CompletedTask;
		}

		public string GetDefaultLayoutXml()
		{
			return "" +
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>"+
                "<LayoutRoot xmlns=\"http://schemas.xceed.com/wpf/xaml/avalondock/layout\">" +
				"  <RootPanel Orientation=\"Horizontal\">" +
				"    <LayoutAnchorablePane DockWidth=\"300\">" +
				"      <LayoutAnchorable Title=\"快捷\" ContentId=\"Tool.Quick\" />" +
				"    </LayoutAnchorablePane>" +
				"    <LayoutDocumentPane>" +
				"      <LayoutDocument Title=\"编辑器\" ContentId=\"Document.Editor\" />" +
				"    </LayoutDocumentPane>" +
				"    <LayoutAnchorablePane DockWidth=\"340\">" +
				"      <LayoutAnchorable Title=\"AI 聊天\" ContentId=\"Tool.AIChat\" />" +
				"    </LayoutAnchorablePane>" +
				"  </RootPanel>" +
				"</LayoutRoot>";
		}

		private static XmlLayoutSerializer CreateSerializerWithCallback(DockingManager dockingManager)
		{
			var serializer = new XmlLayoutSerializer(dockingManager);
			serializer.LayoutSerializationCallback += (s, e) =>
			{
				switch (e.Model.ContentId)
				{
					case "Tool.Quick":
						e.Content = new QuickPanelView();
						break;
					case "Tool.AIChat":
						e.Content = new AIChatView();
						break;
					case "Document.Editor":
						e.Content = new MainEditorView();
						break;
					default:
						e.Content = null; // 保持空内容
						break;
				}
			};
			return serializer;
		}

		private static string NormalizeNamespace(string xml)
		{
			if (string.IsNullOrWhiteSpace(xml)) return xml;
			xml = xml.Replace(NsUrn, NsHttp);
			// 兜底：如果缺失 xmlns，则强制注入（仅当根包含 <LayoutRoot 且无 xmlns）
			if (xml.Contains("<LayoutRoot") && !xml.Contains("xmlns=\""))
			{
				xml = xml.Replace("<LayoutRoot", "<LayoutRoot xmlns=\"" + NsHttp + "\"");
			}
			return xml;
		}

		private static string TrimBom(string text)
		{
			if (string.IsNullOrEmpty(text)) return text;
			// 移除可能的 BOM 字符
			if (text.Length > 0 && text[0] == '\uFEFF')
			{
				return text.TrimStart('\uFEFF');
			}
			return text;
		}

		private static string PreprocessXml(string xml)
		{
			if (string.IsNullOrEmpty(xml)) return xml;
			xml = TrimBom(xml);
			xml = xml.TrimStart('\uFEFF', '\u0000', '\u0009', '\u000A', '\u000D', ' ', '\t');
			// 移除XML声明，部分环境下XmlSerializer对声明和前导空白敏感
			if (xml.StartsWith("<?xml", StringComparison.Ordinal))
			{
				var end = xml.IndexOf("?>", StringComparison.Ordinal);
				if (end > 0)
				{
					xml = xml.Substring(end + 2);
				}
			}
			xml = NormalizeNamespace(xml);
			// 最终再一次去除前导空白确保第一字符即为 '<'
			xml = xml.TrimStart('\uFEFF', '\u0000', '\u0009', '\u000A', '\u000D', ' ', '\t');
			return xml;
		}
	}
}
