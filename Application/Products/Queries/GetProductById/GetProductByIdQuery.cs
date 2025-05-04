// Application/Products/Queries/GetProductById/GetProductByIdQuery.cs
using Application.DTOs;
using Application.Common; // <-- Namespace where OperationResult is located
using MediatR;

namespace Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<OperationResult<ProductDto>>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
