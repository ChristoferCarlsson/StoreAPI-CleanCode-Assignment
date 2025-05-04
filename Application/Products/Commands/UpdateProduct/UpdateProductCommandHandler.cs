using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        _repo.Update(product);
        await _repo.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }
}
