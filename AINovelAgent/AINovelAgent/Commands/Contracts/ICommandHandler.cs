using System.Threading.Tasks;

namespace AINovelAgent.Commands.Contracts
{
    public interface ICommandHandler<TRequest> where TRequest : ICommandRequest
    {
        Task<CommandResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}


