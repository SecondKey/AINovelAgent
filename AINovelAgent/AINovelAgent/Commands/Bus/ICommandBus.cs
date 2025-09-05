using System.Threading.Tasks;
using AINovelAgent.Commands.Contracts;

namespace AINovelAgent.Commands.Bus
{
    public interface ICommandBus
    {
        Task<CommandResult> SendAsync(ICommandRequest request, CancellationToken cancellationToken = default);
    }
}


