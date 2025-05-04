using Application.DTOs;
using Application.Interfaces;
using Application.Common; 
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, OperationResult<ProductDto>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(request.Id);
            if (product == null)
            {
                return OperationResult<ProductDto>.FailureResult($"Product with ID {request.Id} not found.");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.CategoryId = request.CategoryId;

            _repo.Update(product);
            await _repo.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            return OperationResult<ProductDto>.SuccessResult(productDto);
        }
    }
}
