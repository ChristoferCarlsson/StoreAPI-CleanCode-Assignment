using FluentValidation;

namespace Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Category ID must be greater than 0.");
        }
    }
}
