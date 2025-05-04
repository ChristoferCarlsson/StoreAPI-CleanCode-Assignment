using MediatR;
using Application.DTOs;

namespace Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(int Id, string Name, decimal Price, int Stock, int CategoryId)
    : IRequest<ProductDto>;
