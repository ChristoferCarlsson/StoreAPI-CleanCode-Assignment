using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, OperationResult<List<CategoryDto>>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Fetch the categories from the repository
            var categories = await _repo.GetAllAsync();

            // If categories are not found, return a failure result
            if (categories == null || categories.Count == 0)
            {
                return OperationResult<List<CategoryDto>>.FailureResult("No categories found.");
            }

            // Map to DTOs and return success result with data
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return OperationResult<List<CategoryDto>>.SuccessResult(categoryDtos);
        }
    }
}
