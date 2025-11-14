
using MediatR;
using TechHive.Domain.Enums;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;

namespace TechHive.Application.Products.Command.CreateProducts;

public record CreateProductCommand(ProductName Name, ProductCode Code, Money? Price, ProductStatus Status, string? Description) :IRequest<Result<ProductId>>;
