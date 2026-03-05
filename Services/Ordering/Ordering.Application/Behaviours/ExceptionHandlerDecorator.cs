using Microsoft.Extensions.Logging;
using Ordering.Application.Abstactions;

namespace Ordering.Application.Behaviours
{
    public class ExceptionHandlerDecorator<TCommand, TResult>(ICommandHandler<TCommand, TResult> handler, ILogger<TCommand> logger) : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler = handler;
        private readonly ILogger _logger = logger;

        public Task<int> Handle(TCommand command)
        {
            try
            {
                return _handler.Handle(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing the command of type {CommandType} {Command}.", typeof(TCommand).Name, command);
                throw;
            }
        }
    }
}
