using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Application.Common;

namespace Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, OperationResult<ProductDto>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(request.Id);

            if (product == null)
            {
                return OperationResult<ProductDto>.FailureResult("Product not found.");
            }

            var dto = _mapper.Map<ProductDto>(product);
            return OperationResult<ProductDto>.SuccessResult(dto);
        }
    }
}
