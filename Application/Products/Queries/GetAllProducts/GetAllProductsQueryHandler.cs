using Application.DTOs;
using Application.Interfaces;
using Application.Common;
using AutoMapper;
using MediatR;

namespace Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, OperationResult<List<ProductDto>>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repo.GetAllAsync();

            if (products == null || !products.Any())
            {
                return OperationResult<List<ProductDto>>.FailureResult("No products found.");
            }

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return OperationResult<List<ProductDto>>.SuccessResult(productDtos);
        }
    }
}
