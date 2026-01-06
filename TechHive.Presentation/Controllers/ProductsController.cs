
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechHive.Application.Common;
using TechHive.Application.Products.Command.CreateProducts;
using TechHive.Application.Products.Command.DeleteProducts;
using TechHive.Application.Products.Command.UpdateProducts;
using TechHive.Application.Products.Query.GetProducts;
using TechHive.Domain.Enums;
using TechHive.Domain.ValueObjects;
using static TechHive.Presentation.SupabaseFileStorage;


namespace TechHive.Presentation.Controllers;

[Route("[controller]")]
public class ProductController : ApiController
{
    private readonly ISender _sender;
    private readonly HttpClient _httpClient;

    public ProductController(ISender sender, HttpClient httpClient)
    {
        _sender = sender;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(" https://localhost:7141/");
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
    {
        if (request == null)
            return BadRequest("Invalid request payload");

        var nameResult = ProductName.Create(request.Name);
        if (nameResult.IsFailure)
            return BadRequest(nameResult.Error);

        var codeResult = ProductCode.Create(request.Code);
        if (codeResult.IsFailure)
            return BadRequest(codeResult.Error);

        Money? price = null;
        if (request.PriceAmount.HasValue)
        {
            var moneyResult = Money.Create(request.PriceAmount.Value, Currency.FromCode(request.PriceCurrency).Value);
            if (moneyResult.IsFailure)
                return BadRequest(moneyResult.Error);

            price = moneyResult.Value;
        }

        var statusResult = ProductStatus.FromCode(request.Status);

        if (statusResult.IsFailure)
            return BadRequest(statusResult.Error);

        var command = new CreateProductCommand(
            Name: nameResult.Value,
            Code: codeResult.Value,
            Price: price,
            Status: statusResult.Value,
            Description: request.Description
        );

        var result = await _sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { ProductId = result.Value.Value });
    }

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

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
    {
        var UpdateProductresult = await _sender.Send(command);
        return UpdateProductresult.IsSuccess ? Ok(UpdateProductresult) : Problem();

    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (model == null)
            return BadRequest("Invalid request payload");

        var loginResponse = await _httpClient.PostAsJsonAsync("users/login", model);

        if (!loginResponse.IsSuccessStatusCode)
        {
            var errorContent = await loginResponse.Content.ReadAsStringAsync();

            if (loginResponse.StatusCode == System.Net.HttpStatusCode.NotFound ||
                errorContent.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("User not found");
            }

            return StatusCode((int)loginResponse.StatusCode, new { message = "Login failed", detail = errorContent });
        }

        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginData == null || string.IsNullOrEmpty(loginData.AccessToken))
        {
            return StatusCode(500, "Invalid response from authentication service: missing token");
        }

        return Ok(new
        {
            accessToken = loginData.AccessToken,
            role = loginData.role,
            ExpireWhen = loginData.ExpiresIn
        });
    }
}

public record LoginModel(string Email, string Password);
[AuthorizeRoles("Admin")]
public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal? PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "USD";
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}