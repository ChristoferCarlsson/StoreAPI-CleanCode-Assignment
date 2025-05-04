using FluentValidation;

namespace Application.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
    {
        public GetAllCategoriesQueryValidator()
        {
            // Add any logic if needed in the future
            RuleFor(x => x).NotNull().WithMessage("Query cannot be null.");
        }
    }
}
