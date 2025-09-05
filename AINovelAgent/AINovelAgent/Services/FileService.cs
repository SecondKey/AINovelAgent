using AINovelAgent.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace AINovelAgent.Services
{
    /// <summary>
    /// 文件服务实现
    /// </summary>
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> SaveDocumentAsync(FlowDocument document, string filePath, FileFormat format)
        {
            try
            {
                var textRange = new TextRange(document.ContentStart, document.ContentEnd);
                
                string dataFormat = format switch
                {
                    FileFormat.Rtf => DataFormats.Rtf,
                    FileFormat.Xaml => DataFormats.Xaml,
                    FileFormat.Txt => DataFormats.Text,
                    _ => DataFormats.Rtf
                };

                using var fileStream = new FileStream(filePath, FileMode.Create);
                
                if (format == FileFormat.Rtf)
                {
                    await SaveAsRtfAsync(document, fileStream).ConfigureAwait(false);
                }
                else if (format == FileFormat.Xaml)
                {
                    textRange.Save(fileStream, dataFormat);
                }
                else if (format == FileFormat.Txt)
                {
                    await SaveAsPlainTextAsync(document, fileStream).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存文档失败: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<FlowDocument?> LoadDocumentAsync(string filePath, FileFormat format)
        {
            try
            {
                var document = new FlowDocument();
                var textRange = new TextRange(document.ContentStart, document.ContentEnd);
                
                string dataFormat = format switch
                {
                    FileFormat.Rtf => DataFormats.Rtf,
                    FileFormat.Xaml => DataFormats.Xaml,
                    FileFormat.Txt => DataFormats.Text,
                    _ => DataFormats.Rtf
                };

                using var fileStream = new FileStream(filePath, FileMode.Open);
                
                if (format == FileFormat.Rtf || format == FileFormat.Xaml)
                {
                    textRange.Load(fileStream, dataFormat);
                }
                else if (format == FileFormat.Txt)
                {
                    await LoadAsPlainTextAsync(document, fileStream).ConfigureAwait(false);
                }

                return document;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载文档失败: {Message}", ex.Message);
                return null;
            }
        }

        public IEnumerable<FileFormat> GetSupportedFormats()
        {
            return Enum.GetValues<FileFormat>();
        }

        private async Task SaveAsPlainTextAsync(FlowDocument document, FileStream fileStream)
        {
            using var writer = new StreamWriter(fileStream, Encoding.UTF8);
            
            for (int i = 0; i < document.Blocks.Count; i++)
            {
                var block = document.Blocks.ElementAt(i);
                
                if (block is Paragraph paragraph)
                {
                    var paragraphRange = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
                    var text = paragraphRange.Text;
                    
                    await writer.WriteAsync(text).ConfigureAwait(false);
                    await writer.WriteLineAsync().ConfigureAwait(false);
                }
            }
            
            await writer.FlushAsync().ConfigureAwait(false);
        }

        private async Task LoadAsPlainTextAsync(FlowDocument document, FileStream fileStream)
        {
            using var reader = new StreamReader(fileStream, Encoding.UTF8);
            var content = await reader.ReadToEndAsync().ConfigureAwait(false);
            
            document.Blocks.Clear();
            
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            foreach (var line in lines)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(line));
                document.Blocks.Add(paragraph);
            }
        }

        private Task SaveAsRtfAsync(FlowDocument document, FileStream fileStream)
        {
            EnsureProperParagraphStructure(document);
            
            var newDocument = new FlowDocument();
            
            foreach (Block block in document.Blocks)
            {
                if (block is Paragraph originalParagraph)
                {
                    var newParagraph = new Paragraph();
                    
                    foreach (Inline inline in originalParagraph.Inlines)
                    {
                        if (inline is Run run)
                        {
                            var newRun = new Run(run.Text);
                            newRun.FontFamily = run.FontFamily;
                            newRun.FontSize = run.FontSize;
                            newRun.FontWeight = run.FontWeight;
                            newRun.FontStyle = run.FontStyle;
                            newRun.TextDecorations = run.TextDecorations;
                            newRun.Background = run.Background;
                            newRun.Foreground = run.Foreground;
                            newParagraph.Inlines.Add(newRun);
                        }
                    }
                    
                    newParagraph.LineHeight = originalParagraph.LineHeight;
                    newParagraph.Margin = originalParagraph.Margin;
                    newParagraph.TextAlignment = originalParagraph.TextAlignment;
                    
                    newDocument.Blocks.Add(newParagraph);
                }
            }
            
            var textRange = new TextRange(newDocument.ContentStart, newDocument.ContentEnd);
            textRange.Save(fileStream, DataFormats.Rtf);
            return Task.CompletedTask;
        }

        private void EnsureProperParagraphStructure(FlowDocument document)
        {
            if (document.Blocks.Count == 0)
            {
                var emptyParagraph = new Paragraph();
                document.Blocks.Add(emptyParagraph);
                return;
            }

            foreach (Block block in document.Blocks.ToList())
            {
                if (block is Paragraph paragraph && paragraph.Inlines.Count == 0)
                {
                    paragraph.Inlines.Add(new Run());
                }
            }
        }
    }
}
