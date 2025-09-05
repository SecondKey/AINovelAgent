using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AINovelAgent.Commands.Bus;
using AINovelAgent.Commands.Contracts;
using AINovelAgent.Commands.Definitions;
using AINovelAgent.Commands.Requests;

namespace AINovelAgent.Commands.Routing
{
    public sealed class CommandRouter
    {
        private readonly ICommandBus _commandBus;

        public CommandRouter(ICommandBus commandBus)
        {
            _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        }

        public bool CanExecute(ICommand command, object? parameter)
        {
            return true; // 初版：一律可执行；后续可接入状态查询
        }

        public async Task ExecuteAsync(ICommand command, object? parameter, CancellationToken cancellationToken = default)
        {
            ICommandRequest? request = parameter as ICommandRequest;

            // 静态映射：文件命令 → 对应的Command
            if (request is null && ReferenceEquals(command, FileCommands.New))
            {
                request = new NewProjectCommand();
            }
            else if (request is null && ReferenceEquals(command, FileCommands.Open))
            {
                request = new OpenProjectCommand();
            }

            if (request != null)
                await _commandBus.SendAsync(request, cancellationToken);
        }
    }
}


