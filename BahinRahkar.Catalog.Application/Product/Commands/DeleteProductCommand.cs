using Alamut.Abstractions.Structure;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using BehinRahkar.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BahinRahkar.Catalog.Application.Product.Commands
{
    using BehinRahkar.Catalog.Domain.Models;

    public class DeleteProductCommand : IRequest<Result>
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly ICatalogDbContext _dbContext;

        public DeleteProductCommandHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p=> p.Id == command.Id);
                if (product == null) return Result<Product>.Error("Product was not found.");

                var dbProduct = _dbContext.Products.Remove(product);
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

                if (saveResult > 0)
                {
                    return Result.Okay("Product was deleted.");
                }
                return Result.Error("Product was not deleted.");
            }
            catch (Exception exception)
            {
                return Result<Product>.Exception(exception);
            }
        }
    }
}
