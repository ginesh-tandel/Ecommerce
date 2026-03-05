using FluentValidation;
using Ordering.Application.Abstactions;

namespace Ordering.Application.Behaviours
{
    public class ValidationHandlerDecorator<TCommand, TResult>(
        ICommandHandler<TCommand, TResult> handler, IEnumerable<IValidator<TCommand>> validators) : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler = handler;
        private readonly IEnumerable<IValidator<TCommand>> _validators = validators;

        public async Task<int> Handle(TCommand command)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TCommand>(command);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if(failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }
            return await _handler.Handle(command);
        }
    }
}
