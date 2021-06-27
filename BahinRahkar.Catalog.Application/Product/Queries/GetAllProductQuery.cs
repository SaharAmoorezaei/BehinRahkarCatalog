
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BehinRahkar.Application.Contracts;

namespace BehinRahkar.Catalog.Application.Product.Queries
{

    using BehinRahkar.Catalog.Domain.Models;


    public class GetAllProductQuery : IRequest<List<Product>>
    {
        public GetAllProductQuery()
        {
        }
    }

    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<Product>>
    {
        private readonly ICatalogDbContext _dbContext;
        private readonly ILogger<GetAllProductQueryHandler> _logger;
        public GetAllProductQueryHandler(ICatalogDbContext dbContext, ILogger<GetAllProductQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.Where(p=>p.IsConfirmed).ToListAsync(cancellationToken);
        }
    }
}
