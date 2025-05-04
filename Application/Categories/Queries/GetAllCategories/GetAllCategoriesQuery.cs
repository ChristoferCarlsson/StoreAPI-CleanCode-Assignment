using Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Categories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDto>>;
}
