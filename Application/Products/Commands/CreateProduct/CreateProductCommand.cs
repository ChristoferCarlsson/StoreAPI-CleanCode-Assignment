using Application.DTOs;
using MediatR;

namespace Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price, int Stock, int CategoryId)
        : IRequest<OperationResult<ProductDto>>; 
}
