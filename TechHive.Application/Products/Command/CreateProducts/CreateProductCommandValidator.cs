

using FluentValidation;
using TechHive.Domain.Errors.ProductErrors;

namespace TechHive.Application.Products.Command.CreateProducts;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product name is required.")
            .WithState(_ => ProductErrors.Name.Empty);
        RuleFor(x => x.Code)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product code is required.")
            .WithState(_ => ProductErrors.Code.Empty)
            .WithState(_ => ProductErrors.Code.InvalidFormat);

        RuleFor(x => x.Price)
            .NotNull()
            .Must(x => x.Amount > 0)
            .WithErrorCode("400")
            .WithMessage("Product price must be greater than zero.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product description is required.");


    }
}
