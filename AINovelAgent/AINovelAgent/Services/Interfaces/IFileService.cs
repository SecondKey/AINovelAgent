using System.Windows.Documents;

namespace AINovelAgent.Services.Interfaces
{
    /// <summary>
    /// 文件服务接口
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// 保存文档到文件
        /// </summary>
        /// <param name="document">要保存的文档</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="format">文件格式</param>
        /// <returns>保存是否成功</returns>
        Task<bool> SaveDocumentAsync(FlowDocument document, string filePath, FileFormat format);

        /// <summary>
        /// 从文件加载文档
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="format">文件格式</param>
        /// <returns>加载的文档</returns>
        Task<FlowDocument?> LoadDocumentAsync(string filePath, FileFormat format);

        /// <summary>
        /// 获取支持的文件格式
        /// </summary>
        /// <returns>支持的文件格式列表</returns>
        IEnumerable<FileFormat> GetSupportedFormats();
    }

    /// <summary>
    /// 文件格式枚举
    /// </summary>
    public enum FileFormat
    {
        Rtf,
        Xaml,
        Txt
    }
}
