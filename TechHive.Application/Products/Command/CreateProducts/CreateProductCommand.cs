
using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;

namespace TechHive.Application.Products.Command.CreateProducts;

[AuthorizeRoles("Admin")]
public record CreateProductCommand(
    string Name,
    string Code,
    decimal PriceAmount,
    string PriceCurrency,
    string Status,
    string? Description
) : IRequest<Result<ProductId>>;

