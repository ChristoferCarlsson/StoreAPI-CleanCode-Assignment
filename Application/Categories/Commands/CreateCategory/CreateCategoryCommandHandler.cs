// Application/Categories/Commands/CreateCategory/CreateCategoryCommandHandler.cs
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult<CategoryDto>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Check if the category already exists
            var existingCategory = await _repo.GetByNameAsync(request.Name);
            if (existingCategory != null)
            {
                return OperationResult<CategoryDto>.FailureResult("Category already exists.");
            }

            var category = new Category { Name = request.Name };

            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return OperationResult<CategoryDto>.SuccessResult(categoryDto);
        }
    }
}
