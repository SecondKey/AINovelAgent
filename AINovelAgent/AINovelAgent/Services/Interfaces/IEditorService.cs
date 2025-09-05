using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AINovelAgent.Services.Interfaces
{
    /// <summary>
    /// 编辑器服务接口
    /// </summary>
    public interface IEditorService
    {
        /// <summary>
        /// 应用字体格式
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">字体大小</param>
        void ApplyFontFormat(FlowDocument document, FontFamily fontFamily, double fontSize);

        /// <summary>
        /// 应用文本格式
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="fontWeight">字体粗细</param>
        /// <param name="fontStyle">字体样式</param>
        /// <param name="textDecorations">文本装饰</param>
        void ApplyTextFormat(FlowDocument document, FontWeight fontWeight, FontStyle fontStyle, TextDecorationCollection? textDecorations);

        /// <summary>
        /// 应用背景颜色
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="background">背景颜色</param>
        void ApplyBackgroundColor(FlowDocument document, Brush background);

        /// <summary>
        /// 应用行间距
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="lineSpacing">行间距</param>
        void ApplyLineSpacing(FlowDocument document, double lineSpacing);

        /// <summary>
        /// 应用字符间距
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="characterSpacing">字符间距</param>
        void ApplyCharacterSpacing(FlowDocument document, double characterSpacing);

        /// <summary>
        /// 获取文档统计信息
        /// </summary>
        /// <param name="document">文档</param>
        /// <returns>统计信息</returns>
        DocumentStatistics GetDocumentStatistics(FlowDocument document);

        /// <summary>
        /// 获取系统可用字体
        /// </summary>
        /// <returns>字体列表</returns>
        IEnumerable<string> GetAvailableFonts();
    }

    /// <summary>
    /// 文档统计信息
    /// </summary>
    public class DocumentStatistics
    {
        public int CharacterCount { get; set; }
        public int LineCount { get; set; }
        public int ParagraphCount { get; set; }
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
    }
}
