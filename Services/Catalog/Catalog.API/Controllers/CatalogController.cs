using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            var query = new GetAllProductsQuery(catalogSpecParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("GetProductsByName/{name}")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            var query = new GetProductsByNameQuery(name);
            var result = await _mediator.Send(query);
            if (result == null || result.Count == 0) return NotFound();
            var dtoList = result.Select(p => p.ToDto()).ToList();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductIdCommand(id);
            await _mediator.Send(command);
            if (command == null) return NotFound();
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
        {
            var command = updateProductDto.ToCommand(id);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpGet("GetAllBrands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetAllTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetProductByBrand/{brand}")]
        public async Task<IActionResult> GetProductByBrand(string brand)
        {
            var query = new GetProductsByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
