
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechHive.Application.Products.Command.CreateProducts;
using TechHive.Application.Products.Command.DeleteProducts;
using TechHive.Application.Products.Command.UpdateProducts;
using TechHive.Application.Products.Query.GetProducts;
using TechHive.Domain.ValueObjects;


namespace TechHive.Presentation.Controllers;

[Route("[controller]")]
public class ProductsController : ApiController
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand request)
    {
        var CreateProductresult = await _sender.Send(request);
        return Ok(CreateProductresult);

    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{ProductId:int}")]
    public async Task<IActionResult> DeleteProduct(ProductId productId)
    {
        var command = new DeleteProductCommand(productId);
        var DeleteProductresult = await _sender.Send(command);
        return NoContent();

    }
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProduct(ProductId productId)
    {
        var query = new GetProductQuery(productId);
        var GetProductResult = await _sender.Send(query);
        return Ok(GetProductResult);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
    {
        var UpdateProductresult = await _sender.Send(command);
        return UpdateProductresult.IsSuccess ? Ok(UpdateProductresult) : Problem();

    }
}
