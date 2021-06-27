

using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BehinRahkar.Catalog.Infra.Config
{
    public class TraceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public TraceBehavior(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("TraceBehavior");
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogTrace("calling : " + typeof(TRequest).Name);
            return await next();
        }
    }
}
