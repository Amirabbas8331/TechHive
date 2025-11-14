
using MediatR;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;

namespace TechHive.Application.Products.Command.DeleteProducts;

public record DeleteProductCommand(ProductId ProductId):IRequest<Result>;
