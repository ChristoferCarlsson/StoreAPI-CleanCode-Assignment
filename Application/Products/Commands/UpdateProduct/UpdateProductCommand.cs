using Application.DTOs;
using Application.Common;
using MediatR;

namespace Application.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(int Id, string Name, decimal Price, int Stock, int CategoryId)
        : IRequest<OperationResult<ProductDto>>;
}
