// Application/Products/Commands/CreateProduct/CreateProductCommandHandler.cs
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if a product with the same name and category already exists
            var existingProduct = await _repo.GetByNameAndCategoryAsync(request.Name, request.CategoryId);
            if (existingProduct != null)
            {
                throw new Exception("Product with the same name and category already exists.");
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

            return _mapper.Map<ProductDto>(product);
        }
    }
}
