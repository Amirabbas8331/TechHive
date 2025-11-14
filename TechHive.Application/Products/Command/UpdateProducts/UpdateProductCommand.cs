
using MediatR;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Products.Command.UpdateProducts;

public record UpdateProductCommand(Product Product):IRequest<Result<ProductId>>;