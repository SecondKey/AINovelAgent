using System;
using System.Threading.Tasks;
using AINovelAgent.Commands.Contracts;

namespace AINovelAgent.Commands.Bus
{
    public sealed class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<CommandResult> SendAsync(ICommandRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) return CommandResult.Failure("请求为空");

            var requestType = request.GetType();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(requestType);
            var handler = _serviceProvider.GetService(handlerType) as dynamic;
            if (handler == null)
            {
                return CommandResult.Failure($"未找到处理器: {handlerType.Name}");
            }

            try
            {
                CommandResult result = await handler.HandleAsync((dynamic)request, cancellationToken);
                return result ?? CommandResult.Failure("处理结果为空");
            }
            catch (Exception ex)
            {
                return CommandResult.Failure(ex.Message);
            }
        }
    }
}


