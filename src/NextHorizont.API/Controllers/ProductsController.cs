using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Products.Commands.CreateProduct;
using NextHorizont.Application.UseCases.Products.Queries.GetActiveProducts;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{
    public record CreateProductRequest(Guid CategoryId, Guid? LogicalPrinterId, string Name, decimal Price, string? Availability);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand(
            TenantId,
            request.CategoryId,
            request.LogicalPrinterId,
            request.Name,
            request.Price,
            request.Availability);

        var result = await Mediator.Send(command);
        return Ok(new { ProductId = result });
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var query = new GetActiveProductsQuery(TenantId);
        var products = await Mediator.Send(query);

        return Ok(products.Select(p => new
        {
            p.Id,
            p.CategoryId,
            p.Name,
            p.Price,
            p.Availability,
            p.IsActive
        }));
    }
}
