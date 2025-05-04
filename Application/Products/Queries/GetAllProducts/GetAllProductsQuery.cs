using Application.DTOs;
using Application.Common;
using MediatR;

namespace Application.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(int? CategoryId = null) : IRequest<OperationResult<List<ProductDto>>>;
}
