// Application/Products/Queries/GetAllProducts/GetAllProductsQueryValidator.cs

using FluentValidation;

namespace Application.Products.Queries.GetAllProducts;

public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
        // If CategoryId is provided, it must be positive
        RuleFor(q => q.CategoryId)
            .GreaterThan(0)
            .When(q => q.CategoryId.HasValue)
            .WithMessage("CategoryId must be greater than zero.");
    }
}
