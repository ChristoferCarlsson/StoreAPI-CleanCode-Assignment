using Application.Interfaces;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult<bool>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                return OperationResult<bool>.FailureResult("Category not found.");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return OperationResult<bool>.SuccessResult(true); // Deletion successful
        }
    }
}
