using FluentValidation;

namespace Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Category ID must be greater than 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category Name must not be empty.");
        }
    }
}
