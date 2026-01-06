
using MediatR;
using TechHive.Application.Common;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;

namespace TechHive.Application.Products.Command.DeleteProducts;

[AuthorizeRoles("Admin")]
public record DeleteProductCommand(ProductId ProductId) : IRequest<Result>;
