using AINovelAgent.Commands.Contracts;

namespace AINovelAgent.Commands.Requests
{
    /// <summary>
    /// 新建项目命令
    /// </summary>
    public sealed class NewProjectCommand : ICommandRequest
    {
        // 这个命令不需要额外参数，因为所有信息都通过窗口获取
    }
}
