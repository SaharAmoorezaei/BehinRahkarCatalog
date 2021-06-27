
using BehinRahkar.Catalog.Application.Dto;
using BehinRahkar.Catalog.Application.Product.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace BehinRahkar.Catalog.Application.Product.Validators
{

    public static class ProductValidators
    {
        public static IRuleBuilderOptions<T, string> IsDuplicated<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            IMediator mediator)
        {
            return ruleBuilder.MustAsync(async (code, token) =>
            {
                var product = await mediator.Send(new GetProductByCodeQuery(code), token);
                return product == null;
            })
                .WithMessage("There is a product with this code.");
        }

        //public static IRuleBuilderOptions<T, EditProductDto> IsDuplicated<T>(
        //    this IRuleBuilder<T, EditProductDto> ruleBuilder,
        //    IMediator mediator)
        //{
        //    return ruleBuilder.MustAsync(async (p, token) =>
        //    {
        //        var product = await mediator.Send(new GetProductByCodeQuery(p.Code), token);
        //        return product == null || product.Id == p.Id;
        //    })
        //        .WithMessage("There is a product with this code.");
        //}

        public static IRuleBuilderOptions<T, IFormFile> ImageSize<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder.MustAsync(async (f, token) =>
            {
                return f == null || f.Length < 5 * 1024 * 1024;
            })
                .WithMessage("File size is larger than allowed.");
        }

        public static IRuleBuilderOptions<T, IFormFile> ImageType<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder.MustAsync(async (f, token) =>
            {
                var extensions = new string[] { ".jpg", ".png", ".pdf" };
                return f == null || Array.IndexOf(extensions, Path.GetExtension(f.FileName.ToLower())) > -1;
            })
                .WithMessage("Invalid file type.");
        }


    }
}
