
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BehinRahkar.Catalog.Application.Product.Queries
{
    using BehinRahkar.Application.Contracts;
    using BehinRahkar.Catalog.Domain.Models;

    public class GetProductQuery : IRequest<Product>
    {
        public GetProductQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly ICatalogDbContext _dbContext;
        public GetProductQueryHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p=>p.Id == request.Id);
        }
    }
}
