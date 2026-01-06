
using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Products.Command.UpdateProducts;

[AuthorizeRoles("Admin")]
public record UpdateProductCommand(Product Product) : IRequest<Result<ProductId>>;