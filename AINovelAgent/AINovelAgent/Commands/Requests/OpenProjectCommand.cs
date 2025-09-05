using AINovelAgent.Commands.Contracts;

namespace AINovelAgent.Commands.Requests
{
    public sealed class OpenProjectCommand : ICommandRequest
    {
        public string? InitialDirectory { get; }

        public OpenProjectCommand(string? initialDirectory = null)
        {
            InitialDirectory = initialDirectory;
        }
    }
}


