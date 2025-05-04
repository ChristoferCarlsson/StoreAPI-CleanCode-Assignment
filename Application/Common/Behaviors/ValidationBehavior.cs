using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // If validation fails, throw an exception or handle accordingly
                throw new ValidationException(validationResult.Errors);
            }

            // If validation passes, call the next handler in the pipeline
            return await next();
        }
    }
}
