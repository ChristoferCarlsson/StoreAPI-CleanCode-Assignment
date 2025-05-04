using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, OperationResult<CategoryDto>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);

            // If category not found, return failure result
            if (category == null)
            {
                return OperationResult<CategoryDto>.FailureResult($"Category with ID {request.Id} not found.");
            }

            // Map to DTO and return success result
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return OperationResult<CategoryDto>.SuccessResult(categoryDto);
        }
    }
}
