using Application.DTOs;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<OperationResult<CategoryDto>>
    {
        public int Id { get; set; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
