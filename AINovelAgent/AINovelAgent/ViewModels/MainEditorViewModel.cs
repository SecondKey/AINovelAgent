using AINovelAgent.Services.Interfaces;
using AINovelAgent.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AINovelAgent.ViewModels
{
    /// <summary>
    /// 主编辑器视图模型
    /// </summary>
    public partial class MainEditorViewModel : ViewModelBase
    {
        private readonly IFileService _fileService;
        private readonly IEditorService _editorService;
        private readonly IDialogService _dialogService;

        private FlowDocument _document = new();

        public FlowDocument Document
        {
            get => _document;
            set
            {
                if (SetProperty(ref _document, value))
                {
                    // 当Document改变时，可以在这里添加额外的逻辑
                    OnPropertyChanged(nameof(Document));
                }
            }
        }

        [ObservableProperty]
        private string _statusText = "行 1, 列 1    字数 0    段落 0";

        [ObservableProperty]
        private string _selectedFontFamily = "Microsoft YaHei";

        [ObservableProperty]
        private double _selectedFontSize = 14;

        [ObservableProperty]
        private double _selectedLineSpacing = 1.0;

        [ObservableProperty]
        private double _selectedCharacterSpacing = 0;

        [ObservableProperty]
        private bool _isBold;

        [ObservableProperty]
        private bool _isItalic;

        [ObservableProperty]
        private bool _isUnderline;

        public ObservableCollection<string> AvailableFonts { get; } = new();
        public ObservableCollection<double> FontSizes { get; } = new();
        public ObservableCollection<double> LineSpacings { get; } = new();
        public ObservableCollection<double> CharacterSpacings { get; } = new();

        public MainEditorViewModel(IFileService fileService, IEditorService editorService, IDialogService dialogService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _editorService = editorService ?? throw new ArgumentNullException(nameof(editorService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            Title = "编辑器";
            InitializeCollections();
            SetDefaultContent();
        }

        private void InitializeCollections()
        {
            // 初始化字体列表
            var fonts = _editorService.GetAvailableFonts();
            foreach (var font in fonts)
            {
                AvailableFonts.Add(font);
            }

            // 初始化字体大小列表
            var fontSizes = new[] { 10, 12, 14, 16, 18, 20, 24, 28, 32 };
            foreach (var size in fontSizes)
            {
                FontSizes.Add(size);
            }

            // 初始化行间距列表
            var lineSpacings = new[] { 1.0, 1.5, 2.0, 2.5 };
            foreach (var spacing in lineSpacings)
            {
                LineSpacings.Add(spacing);
            }

            // 初始化字符间距列表
            var characterSpacings = new[] { -1.0, 0.0, 2.0, 4.0 };
            foreach (var spacing in characterSpacings)
            {
                CharacterSpacings.Add(spacing);
            }
        }

        private void SetDefaultContent()
        {
            Document.Blocks.Clear();
            
            var paragraphs = new[]
            {
                "欢迎使用AI小说写作助手！",
                "",
                "这是一个功能强大的富文本编辑器，支持：",
                "• 字体和字体大小设置",
                "• 粗体、斜体、下划线格式", 
                "• 背景颜色和高亮功能",
                "• 行间距和字间距调整",
                "• 撤销重做功能",
                "• 多种格式保存",
                "",
                "开始您的创作之旅吧！"
            };
            
            foreach (var text in paragraphs)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(text));
                Document.Blocks.Add(paragraph);
            }

            UpdateStatus();
        }

        [RelayCommand]
        private void UpdateStatus()
        {
            var stats = _editorService.GetDocumentStatistics(Document);
            StatusText = $"行 {stats.CurrentLine}, 列 {stats.CurrentColumn}    字数 {stats.CharacterCount}    段落 {stats.ParagraphCount}";
        }

        [RelayCommand]
        private void ApplyFontFamily()
        {
            var fontFamily = new FontFamily(SelectedFontFamily);
            _editorService.ApplyFontFormat(Document, fontFamily, SelectedFontSize);
        }

        [RelayCommand]
        private void ApplyFontSize()
        {
            var fontFamily = new FontFamily(SelectedFontFamily);
            _editorService.ApplyFontFormat(Document, fontFamily, SelectedFontSize);
        }

        [RelayCommand]
        private void ToggleBold()
        {
            IsBold = !IsBold;
            ApplyTextFormat();
        }

        [RelayCommand]
        private void ToggleItalic()
        {
            IsItalic = !IsItalic;
            ApplyTextFormat();
        }

        [RelayCommand]
        private void ToggleUnderline()
        {
            IsUnderline = !IsUnderline;
            ApplyTextFormat();
        }

        [RelayCommand]
        private void ApplyHighlight()
        {
            _editorService.ApplyBackgroundColor(Document, Brushes.Yellow);
        }

        [RelayCommand]
        private void ApplyBackgroundColor()
        {
            var result = _dialogService.ShowMessageBox(
                "请选择背景颜色：\n1. 黄色\n2. 浅蓝色\n3. 浅绿色\n4. 浅粉色\n5. 浅青色\n6. 浅灰色\n7. 白色\n8. 透明",
                "选择颜色", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.OK)
            {
                _editorService.ApplyBackgroundColor(Document, Brushes.Yellow);
            }
        }

        [RelayCommand]
        private void ApplyLineSpacing()
        {
            _editorService.ApplyLineSpacing(Document, SelectedLineSpacing);
        }

        [RelayCommand]
        private void ApplyCharacterSpacing()
        {
            _editorService.ApplyCharacterSpacing(Document, SelectedCharacterSpacing);
        }

        [RelayCommand]
        private async Task SaveDocument()
        {
            var filePath = _dialogService.ShowSaveFileDialog(
                "RTF文件 (*.rtf)|*.rtf|XAML文件 (*.xaml)|*.xaml|纯文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
                "rtf");

            if (!string.IsNullOrEmpty(filePath))
            {
                var format = Path.GetExtension(filePath).ToLower() switch
                {
                    ".rtf" => FileFormat.Rtf,
                    ".xaml" => FileFormat.Xaml,
                    ".txt" => FileFormat.Txt,
                    _ => FileFormat.Rtf
                };

                var success = await _fileService.SaveDocumentAsync(Document, filePath, format);
                
                if (success)
                {
                    _dialogService.ShowMessageBox("文档保存成功！", "保存", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _dialogService.ShowMessageBox("保存失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private async Task LoadDocument()
        {
            var filePath = _dialogService.ShowOpenFileDialog(
                "RTF文件 (*.rtf)|*.rtf|XAML文件 (*.xaml)|*.xaml|纯文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*");

            if (!string.IsNullOrEmpty(filePath))
            {
                var format = Path.GetExtension(filePath).ToLower() switch
                {
                    ".rtf" => FileFormat.Rtf,
                    ".xaml" => FileFormat.Xaml,
                    ".txt" => FileFormat.Txt,
                    _ => FileFormat.Rtf
                };

                var document = await _fileService.LoadDocumentAsync(filePath, format);
                
                if (document != null)
                {
                    Document = document;
                    _dialogService.ShowMessageBox("文档加载成功！", "加载", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _dialogService.ShowMessageBox("加载失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private void FindText()
        {
            _dialogService.ShowMessageBox("查找功能待实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        private void ReplaceText()
        {
            _dialogService.ShowMessageBox("替换功能待实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplyTextFormat()
        {
            var fontWeight = IsBold ? FontWeights.Bold : FontWeights.Normal;
            var fontStyle = IsItalic ? FontStyles.Italic : FontStyles.Normal;
            var textDecorations = IsUnderline ? TextDecorations.Underline : null;

            _editorService.ApplyTextFormat(Document, fontWeight, fontStyle, textDecorations);
        }
    }
}
