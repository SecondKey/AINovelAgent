using AINovelAgent.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AINovelAgent.Services
{
    /// <summary>
    /// 编辑器服务实现
    /// </summary>
    public class EditorService : IEditorService
    {
        private readonly ILogger<EditorService> _logger;

        public EditorService(ILogger<EditorService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void ApplyFontFormat(FlowDocument document, FontFamily fontFamily, double fontSize)
        {
            var selection = GetSelection(document);
            if (selection.IsEmpty)
            {
                document.FontFamily = fontFamily;
                document.FontSize = fontSize;
            }
            else
            {
                var textRange = new TextRange(selection.Start, selection.End);
                textRange.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
                textRange.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
        }

        public void ApplyTextFormat(FlowDocument document, FontWeight fontWeight, FontStyle fontStyle, TextDecorationCollection? textDecorations)
        {
            var selection = GetSelection(document);
            if (selection.IsEmpty) return;

            var textRange = new TextRange(selection.Start, selection.End);
            textRange.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
            textRange.ApplyPropertyValue(TextElement.FontStyleProperty, fontStyle);
            
            if (textDecorations != null)
            {
                textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        public void ApplyBackgroundColor(FlowDocument document, Brush background)
        {
            var selection = GetSelection(document);
            if (selection.IsEmpty) return;

            var textRange = new TextRange(selection.Start, selection.End);
            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, background);
        }

        public void ApplyLineSpacing(FlowDocument document, double lineSpacing)
        {
            var selection = GetSelection(document);
            if (selection.IsEmpty) return;

            var textRange = new TextRange(selection.Start, selection.End);
            var paragraph = textRange.Start.Paragraph;
            
            if (paragraph != null)
            {
                paragraph.LineHeight = lineSpacing * 14; // 基于14号字体计算
            }
        }

        public void ApplyCharacterSpacing(FlowDocument document, double characterSpacing)
        {
            var selection = GetSelection(document);
            if (selection.IsEmpty) return;

            var textRange = new TextRange(selection.Start, selection.End);
            var currentFontSize = textRange.GetPropertyValue(TextElement.FontSizeProperty);
            
            if (currentFontSize is double fontSize)
            {
                var newFontSize = fontSize + (characterSpacing * 0.5);
                textRange.ApplyPropertyValue(TextElement.FontSizeProperty, Math.Max(8, newFontSize));
            }
        }

        public DocumentStatistics GetDocumentStatistics(FlowDocument document)
        {
            var textRange = new TextRange(document.ContentStart, document.ContentEnd);
            var text = textRange.Text;
            
            return new DocumentStatistics
            {
                CharacterCount = text.Length,
                LineCount = document.Blocks.Count,
                ParagraphCount = document.Blocks.OfType<Paragraph>().Count(),
                CurrentLine = GetCurrentLineNumber(document),
                CurrentColumn = GetCurrentColumnNumber(document)
            };
        }

        public IEnumerable<string> GetAvailableFonts()
        {
            var fonts = new List<string>();
            
            try
            {
                var fontFamilies = Fonts.SystemFontFamilies;
                var sortedFonts = fontFamilies
                    .Select(f => f.Source)
                    .Where(f => !string.IsNullOrEmpty(f))
                    .OrderBy(f => f)
                    .ToList();
                
                var preferredFonts = new[] { "Microsoft YaHei", "SimSun", "SimHei", "KaiTi", "FangSong", "Consolas", "Segoe UI", "Arial", "Times New Roman" };
                
                foreach (var font in preferredFonts)
                {
                    if (sortedFonts.Contains(font))
                    {
                        fonts.Add(font);
                    }
                }
                
                foreach (var font in sortedFonts)
                {
                    if (!fonts.Contains(font))
                    {
                        fonts.Add(font);
                    }
                }
            }
            catch (Exception ex)
            {
                fonts = new List<string> { "Microsoft YaHei", "SimSun", "Consolas", "Arial" };
                _logger.LogError(ex, "获取系统字体失败: {Message}", ex.Message);
            }
            
            return fonts;
        }

        private TextRange GetSelection(FlowDocument document)
        {
            // 这里需要从UI层获取当前选择，暂时返回空选择
            // 在实际实现中，需要通过依赖注入或其他方式获取当前选择
            return new TextRange(document.ContentStart, document.ContentStart);
        }

        private int GetCurrentLineNumber(FlowDocument document)
        {
            // 简化实现，实际应该基于当前选择位置
            return 1;
        }

        private int GetCurrentColumnNumber(FlowDocument document)
        {
            // 简化实现，实际应该基于当前选择位置
            return 1;
        }
    }
}
