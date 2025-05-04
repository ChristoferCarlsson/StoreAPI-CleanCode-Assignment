using FluentValidation;

namespace Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name cannot be empty.");
            RuleFor(x => x.Name).Length(2, 100).WithMessage("Category name must be between 2 and 100 characters.");
        }
    }
}
