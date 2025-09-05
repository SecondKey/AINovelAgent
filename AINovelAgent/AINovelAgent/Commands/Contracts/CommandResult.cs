namespace AINovelAgent.Commands.Contracts
{
    public sealed class CommandResult
    {
        public bool IsSuccess { get; }
        public string? Message { get; }

        private CommandResult(bool isSuccess, string? message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static CommandResult Success(string? message = null) => new(true, message);
        public static CommandResult Failure(string? message = null) => new(false, message);
    }
}


