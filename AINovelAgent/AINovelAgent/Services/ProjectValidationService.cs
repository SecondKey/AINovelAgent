using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using AINovelAgent.Services.Interfaces;

namespace AINovelAgent.Services
{
    /// <summary>
    /// 项目验证服务实现
    /// </summary>
    public class ProjectValidationService : IProjectValidationService
    {
        /// <summary>
        /// 验证指定路径是否为有效的 NovelAgent 项目文件夹
        /// </summary>
        /// <param name="folderPath">要验证的文件夹路径</param>
        /// <returns>验证结果</returns>
        public ProjectValidationResult ValidateProject(string folderPath)
        {
            var result = new ProjectValidationResult();

            try
            {
                // 检查文件夹是否存在
                if (!Directory.Exists(folderPath))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "指定的文件夹不存在";
                    return result;
                }

                // 第一层验证：检查核心标识文件 ProjectSettings.xml
                var projectSettingsPath = Path.Combine(folderPath, "ProjectSettings.xml");
                if (!File.Exists(projectSettingsPath))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "这不是一个有效的 NovelAgent 项目文件夹。\n缺少核心文件：ProjectSettings.xml";
                    return result;
                }

                // 验证 ProjectSettings.xml 格式并提取信息
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(projectSettingsPath);

                    var root = doc.DocumentElement;
                    if (root?.Name != "ProjectSettings")
                    {
                        result.IsValid = false;
                        result.ErrorMessage = "ProjectSettings.xml 文件格式不正确：根节点不是 ProjectSettings";
                        return result;
                    }

                    // 提取项目信息
                    var basicInfo = root.SelectSingleNode("BasicInfo");
                    if (basicInfo != null)
                    {
                        result.ProjectTitle = basicInfo.SelectSingleNode("Title")?.InnerText;
                        result.ProjectAuthor = basicInfo.SelectSingleNode("Author")?.InnerText;
                    }
                }
                catch (XmlException ex)
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"ProjectSettings.xml 文件格式错误：{ex.Message}";
                    return result;
                }

                // 第二层验证：检查推荐文件和文件夹
                CheckRecommendedFiles(folderPath, result.Warnings);

                // 验证通过
                result.IsValid = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"验证过程中发生错误：{ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// 检查推荐的文件和文件夹，生成警告信息
        /// </summary>
        /// <param name="folderPath">项目文件夹路径</param>
        /// <param name="warnings">警告信息列表</param>
        private void CheckRecommendedFiles(string folderPath, List<string> warnings)
        {
            // 检查 metadata.json
            var metadataPath = Path.Combine(folderPath, "metadata.json");
            if (!File.Exists(metadataPath))
            {
                warnings.Add("建议添加 metadata.json 文件以存储项目元数据");
            }

            // 检查 Concepts 文件夹
            var conceptsPath = Path.Combine(folderPath, "Concepts");
            if (!Directory.Exists(conceptsPath))
            {
                warnings.Add("建议创建 Concepts 文件夹以管理小说概念实体");
            }

            // 检查 Chapters 文件夹
            var chaptersPath = Path.Combine(folderPath, "Chapters");
            if (!Directory.Exists(chaptersPath))
            {
                warnings.Add("建议创建 Chapters 文件夹以存储章节内容");
            }

            // 检查其他推荐文件
            var recommendedFiles = new[]
            {
                ("outline.xml", "大纲文件"),
                ("timeline.xml", "时间线文件"),
                ("settings.xml", "世界观设定文件")
            };

            foreach (var (fileName, description) in recommendedFiles)
            {
                var filePath = Path.Combine(folderPath, fileName);
                if (!File.Exists(filePath))
                {
                    warnings.Add($"建议添加 {fileName} ({description})");
                }
            }
        }
    }
}
