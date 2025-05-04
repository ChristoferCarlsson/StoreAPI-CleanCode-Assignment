using Application.DTOs;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UpdateCategoryCommand(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
