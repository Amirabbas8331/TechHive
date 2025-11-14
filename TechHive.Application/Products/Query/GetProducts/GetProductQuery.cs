
using MediatR;
using TechHive.Domain.Results;
using TechHive.Domain.ValueObjects;
using TechHive.Model;

namespace TechHive.Application.Products.Query.GetProducts;

public record GetProductQuery(ProductId ProductId):IRequest<Product>;
