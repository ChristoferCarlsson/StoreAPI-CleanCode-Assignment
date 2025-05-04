using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OperationResult<CategoryDto>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);
            if (category == null)
            {
                return OperationResult<CategoryDto>.FailureResult($"Category with ID {request.Id} not found.");
            }

            // Update category properties
            category.Name = request.Name;

            // Save changes
            await _repo.SaveChangesAsync();

            // Map the updated entity to a DTO
            var updatedCategoryDto = _mapper.Map<CategoryDto>(category);
            return OperationResult<CategoryDto>.SuccessResult(updatedCategoryDto);
        }
    }
}
