using System;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Abstractions.Structure;
using BehinRahkar.Catalog.Application.Product.Queries;
using BehinRahkar.Catalog.Application.Product.Validators;
using Microsoft.Extensions.Logging;
using FluentValidation;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BehinRahkar.Catalog.Application.Product.Commands
{
    using BehinRahkar.Application.Contracts;
    using BehinRahkar.Catalog.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class UpdateProductCommand : IRequest<Result<Product>>
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        
        public string Name { get; set; }

        public IFormFile Photo { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price must be geater than zero")]
        public decimal? Price { get; set; }
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IMediator mediator)
        {
            RuleFor(x => x.Photo).ImageSize();
            RuleFor(x => x.Photo).ImageType();
        }
    }

    public class EditProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Product>>
    {


        private readonly ICatalogDbContext _dbContext;
        private ILogger<EditProductCommandHandler> _logger;

        public EditProductCommandHandler(ICatalogDbContext dbContext, ILogger<EditProductCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<Product>> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id);
                if (product == null) return Result<Product>.Error("Product was not found.");

                product.ChangeEditableProperties(command.Name, command.Photo?.FileName, command.Price);

                var dbProduct = _dbContext.Products.Update(product);
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

                if (saveResult > 0)
                {
                    return Result<Product>.Okay(dbProduct.Entity);
                }

                return Result<Product>.Error("Product was not updated");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Exception was occured in update product with id:{command.Id}=>{exception.Message}");
                throw;
            }
         }
    }
}
