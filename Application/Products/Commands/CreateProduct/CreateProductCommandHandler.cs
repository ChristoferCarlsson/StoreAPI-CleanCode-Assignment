// Application/Products/Commands/CreateProduct/CreateProductCommandHandler.cs
using Application.DTOs;
using Application.Interfaces;
using Application.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading.Tasks;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, OperationResult<ProductDto>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if a product with the same name and category already exists
            var existingProduct = await _repo.GetByNameAndCategoryAsync(request.Name, request.CategoryId);
            if (existingProduct != null)
            {
                return OperationResult<ProductDto>.FailureResult("Product with the same name and category already exists.");
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            };

            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            return OperationResult<ProductDto>.SuccessResult(productDto);
        }
    }
}
