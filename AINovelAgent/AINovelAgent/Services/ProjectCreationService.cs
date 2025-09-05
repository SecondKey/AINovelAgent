using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json;
using AINovelAgent.Services.Interfaces;
using AINovelAgent.ViewModels.Windows;

namespace AINovelAgent.Services
{
    /// <summary>
    /// 项目创建服务实现
    /// </summary>
    public class ProjectCreationService : IProjectCreationService
    {
        /// <summary>
        /// 创建新的小说项目
        /// </summary>
        /// <param name="config">项目配置</param>
        /// <returns>创建结果</returns>
        public async Task<ProjectCreationResult> CreateProjectAsync(ProjectCreationConfig config)
        {
            try
            {
                // 验证配置
                if (string.IsNullOrWhiteSpace(config.ProjectPath))
                    return ProjectCreationResult.Failure("项目路径不能为空");

                if (string.IsNullOrWhiteSpace(config.ProjectName))
                    return ProjectCreationResult.Failure("项目名称不能为空");

                if (string.IsNullOrWhiteSpace(config.AuthorName))
                    return ProjectCreationResult.Failure("作者名称不能为空");

                // 创建项目目录结构
                await CreateProjectStructureAsync(config);

                // 创建核心配置文件
                await CreateProjectSettingsAsync(config);
                await CreateMetadataAsync(config);
                await CreateInitialFilesAsync(config);

                return ProjectCreationResult.Success(config.ProjectPath);
            }
            catch (Exception ex)
            {
                return ProjectCreationResult.Failure($"创建项目时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 创建项目目录结构
        /// </summary>
        /// <param name="config">项目配置</param>
        private async Task CreateProjectStructureAsync(ProjectCreationConfig config)
        {
            await Task.Run(() =>
            {
                // 创建主项目目录
                Directory.CreateDirectory(config.ProjectPath);

                // 创建核心目录结构（简化版）
                var directories = new[]
                {
                    "Concepts",
                    "Concepts/Characters",  // 角色
                    "Concepts/Locations",   // 地点
                    "Events",               // 事件
                    "Chapters",             // 章节
                    ".kb",                  // 知识库
                    ".kb/chroma_vectors",
                    ".kb/lucene_index", 
                    "Templates",            // 模板
                    "Logs"                  // 日志
                };

                foreach (var dir in directories)
                {
                    Directory.CreateDirectory(Path.Combine(config.ProjectPath, dir));
                }
            });
        }

        /// <summary>
        /// 创建项目设置文件
        /// </summary>
        /// <param name="config">项目配置</param>
        private async Task CreateProjectSettingsAsync(ProjectCreationConfig config)
        {
            await Task.Run(() =>
            {
                var doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

                var root = doc.CreateElement("ProjectSettings");
                doc.AppendChild(root);

                // 基本信息
                var basicInfo = doc.CreateElement("BasicInfo");
                root.AppendChild(basicInfo);

                AddElement(doc, basicInfo, "Title", config.ProjectName);
                AddElement(doc, basicInfo, "Author", config.AuthorName);
                AddElement(doc, basicInfo, "Genre", config.ProjectGenre);
                AddElement(doc, basicInfo, "Status", "创作中");
                AddElement(doc, basicInfo, "CreatedDate", DateTime.Now.ToString("yyyy-MM-dd"));
                AddElement(doc, basicInfo, "LastModified", DateTime.Now.ToString("yyyy-MM-dd"));
                AddElement(doc, basicInfo, "Version", "1.0.0");

                // 写作设置
                var writingSettings = doc.CreateElement("WritingSettings");
                root.AppendChild(writingSettings);

                AddElement(doc, writingSettings, "TargetWordCount", config.TargetWordCount.ToString());
                AddElement(doc, writingSettings, "ChapterCount", config.ChapterCount.ToString());
                AddElement(doc, writingSettings, "WordsPerChapter", (config.TargetWordCount / config.ChapterCount).ToString());
                AddElement(doc, writingSettings, "WritingStyle", config.WritingStyle);
                AddElement(doc, writingSettings, "NarrativePerspective", config.NarrativePerspective);
                AddElement(doc, writingSettings, "Tone", "积极向上");

                // AI设置
                var aiSettings = doc.CreateElement("AISettings");
                root.AppendChild(aiSettings);

                AddElement(doc, aiSettings, "ModelName", "llama3.1");
                AddElement(doc, aiSettings, "Temperature", "0.7");
                AddElement(doc, aiSettings, "MaxTokens", "2000");
                AddElement(doc, aiSettings, "EnableRealTimeCorrection", "true");
                AddElement(doc, aiSettings, "EnableStyleConsistency", "true");

                // 导出设置
                var exportSettings = doc.CreateElement("ExportSettings");
                root.AppendChild(exportSettings);

                AddElement(doc, exportSettings, "DefaultFormat", "PDF");
                AddElement(doc, exportSettings, "IncludeImages", "true");
                AddElement(doc, exportSettings, "IncludeMetadata", "true");
                AddElement(doc, exportSettings, "AutoBackup", "true");

                // 保存文件
                var filePath = Path.Combine(config.ProjectPath, "ProjectSettings.xml");
                doc.Save(filePath);
            });
        }

        /// <summary>
        /// 创建元数据文件
        /// </summary>
        /// <param name="config">项目配置</param>
        private async Task CreateMetadataAsync(ProjectCreationConfig config)
        {
            await Task.Run(() =>
            {
                var metadata = new
                {
                    title = config.ProjectName,
                    author = config.AuthorName,
                    genre = config.ProjectGenre,
                    status = "创作中",
                    createdDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    lastModified = DateTime.Now.ToString("yyyy-MM-dd"),
                    version = "1.0.0",
                    wordCount = 0,
                    chapterCount = 0,
                    targetWordCount = config.TargetWordCount,
                    description = config.ProjectDescription,
                    tags = new[] { config.ProjectGenre, config.WritingStyle },
                    language = "zh-CN",
                    encoding = "UTF-8"
                };

                var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                var filePath = Path.Combine(config.ProjectPath, "metadata.json");
                File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
            });
        }

        /// <summary>
        /// 创建初始文件
        /// </summary>
        /// <param name="config">项目配置</param>
        private async Task CreateInitialFilesAsync(ProjectCreationConfig config)
        {
            await Task.Run(() =>
            {
                // 创建空的大纲文件
                CreateEmptyXmlFile(config.ProjectPath, "outline.xml", "Outline");

                // 创建空的时间线文件
                CreateEmptyXmlFile(config.ProjectPath, "timeline.xml", "Timeline");

                // 创建空的设定文件
                CreateEmptyXmlFile(config.ProjectPath, "settings.xml", "Settings");

                // 创建空的分析结果文件
                CreateEmptyXmlFile(config.ProjectPath, "analysis_results.xml", "AnalysisResults");

                // 创建空的创作日志文件
                CreateEmptyXmlFile(config.ProjectPath, "creation_log.xml", "CreationLog");

                // 创建写作指导规则文件
                CreateEmptyXmlFile(config.ProjectPath, "writing_guidelines.xml", "WritingGuidelines");

                // 创建模板文件
                CreateTemplateFiles(config.ProjectPath);
            });
        }

        /// <summary>
        /// 创建空的XML文件
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="rootElement">根元素名称</param>
        private void CreateEmptyXmlFile(string projectPath, string fileName, string rootElement)
        {
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            
            var root = doc.CreateElement(rootElement);
            doc.AppendChild(root);

            var filePath = Path.Combine(projectPath, fileName);
            doc.Save(filePath);
        }

        /// <summary>
        /// 创建模板文件
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        private void CreateTemplateFiles(string projectPath)
        {
            var templatesPath = Path.Combine(projectPath, "Templates");

            // 章节模板
            var chapterTemplate = @"# 第{章节序号}章 {章节标题}

## 章节大纲
- 主要情节：
- 出场角色：
- 发生地点：
- 时间设定：

## 章节内容

{在此处开始写作章节内容}

## 章节总结
- 字数统计：
- 推进情节：
- 角色发展：
- 下章预告：
";
            File.WriteAllText(Path.Combine(templatesPath, "chapter_template.txt"), chapterTemplate, System.Text.Encoding.UTF8);

            // 创建角色、事件、地点模板的XML文件
            CreateEmptyXmlFile(templatesPath, "character_template.xml", "Character");
            CreateEmptyXmlFile(templatesPath, "event_template.xml", "Event");
            CreateEmptyXmlFile(templatesPath, "location_template.xml", "Location");
        }

        /// <summary>
        /// 添加XML元素
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <param name="parent">父元素</param>
        /// <param name="name">元素名称</param>
        /// <param name="value">元素值</param>
        private void AddElement(XmlDocument doc, XmlElement parent, string name, string value)
        {
            var element = doc.CreateElement(name);
            element.InnerText = value;
            parent.AppendChild(element);
        }
    }
}
