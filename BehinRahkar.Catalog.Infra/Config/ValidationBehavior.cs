using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BehinRahkar.Catalog.Infra.Config
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _validators = serviceProvider.GetServices<IValidator<TRequest>>();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var validator in _validators)
            {
                _loggerFactory
                    .CreateLogger("ValidationBehavior")
                    .LogTrace($"ValidationBehavior validate {{{typeof(TRequest).Name}}} with validator {{{validator.GetType().Name}}}");

                await validator.ValidateAndThrowAsync<TRequest>(request, cancellationToken: cancellationToken);
            }

            return await next();
        }
    }
}
