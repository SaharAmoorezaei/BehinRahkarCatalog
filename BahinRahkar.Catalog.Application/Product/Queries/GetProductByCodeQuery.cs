
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BehinRahkar.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BehinRahkar.Catalog.Application.Product.Queries
{

    using BehinRahkar.Catalog.Domain.Models;


    public class GetProductByCodeQuery : IRequest<Product>
    {
        public GetProductByCodeQuery(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }

    public class GetProductByCodeQueryHandler : IRequestHandler<GetProductByCodeQuery, Product>
    {
        private readonly ICatalogDbContext _dbContext;

        public GetProductByCodeQueryHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> Handle(GetProductByCodeQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Code == request.Code);
        }
    }
}
