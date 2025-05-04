using Application.DTOs;
using MediatR;
using Application.Common; 

namespace Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price, int Stock, int CategoryId)
        : IRequest<OperationResult<ProductDto>>;  
}
