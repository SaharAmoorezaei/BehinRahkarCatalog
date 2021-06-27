
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Alamut.Abstractions.Structure;
using BehinRahkar.Catalog.Application.Product.Validators;
using Microsoft.Extensions.Logging;

namespace BehinRahkar.Catalog.Application.Product.Commands
{
    using BehinRahkar.Application.Contracts;
    using BehinRahkar.Catalog.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class CreateProductCommand : IRequest<Result<Product>>
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price must be geater than zero")]
        public decimal Price { get; set; }


        public static implicit operator Product(CreateProductCommand productDto)
        {
            return new Product(productDto.Code, productDto.Name, productDto.Photo?.FileName, productDto.Price);
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IMediator mediator)
        {   
            RuleFor(x => x.Photo).ImageSize();
            RuleFor(x => x.Photo).ImageType();
            RuleFor(x => x.Code).IsDuplicated(mediator);
        }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        private readonly ICatalogDbContext _dbContext;
        private ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(ICatalogDbContext dbContext, ILogger<CreateProductCommandHandler> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var dbProduct = await _dbContext.Products.AddAsync(command, cancellationToken);
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

                if (saveResult > 0)
                {
                    return Result<Product>.Okay(dbProduct.Entity);
                }
                return Result<Product>.Error($"Sorry! product with code:{command.Code} can not be saved.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Exception was occured in saveing product with code:{command.Code} => {exception.Message}");
                throw;
            }
        }
    }
}
