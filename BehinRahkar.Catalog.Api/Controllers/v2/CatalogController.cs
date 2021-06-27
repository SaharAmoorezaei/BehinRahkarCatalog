using Alamut.Abstractions.Structure;
using AutoMapper;
using BahinRahkar.Catalog.Application.Product.Commands;
using BehinRahkar.Catalog.Application.Dto;
using BehinRahkar.Catalog.Application.Product.Commands;
using BehinRahkar.Catalog.Application.Product.Queries;
using BehinRahkar.Catalog.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BehinRahkar.Catalog.Api.Controllers.v2
{

    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/Catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CatalogController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Result<List<ProductDtoV2>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductQuery());
            var products = _mapper.Map<List<ProductDtoV2>>(result);
            return Result<List<ProductDtoV2>>.Okay(products);
        }

        [HttpGet("{id}")]
        public async Task<Result<ProductDtoV2>> Get(int id)
        {
            var result = await _mediator.Send(new GetProductQuery(id));
            var product = _mapper.Map<ProductDtoV2>(result);
            return Result<ProductDtoV2>.Okay(product);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Product>>> Post([FromForm]CreateProductCommand command)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return await _mediator.Send(command);
        }

        [HttpPut]
        public async Task<ActionResult<Result<Product>>> Edit([FromForm]UpdateProductCommand command)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var productResult = await _mediator.Send(command);
            if (productResult && command.Photo != null)
            {
                var photoResult = await _mediator.Send(new SaveFileCommand(command.Photo, productResult.Data.Photo));
                if (!photoResult) productResult.Message = "Product was saved but its photo was not saved please edit product later to save it.";
            }
            return productResult;
        }


        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            return await _mediator.Send(new DeleteProductCommand(id));
        }
    }
}