﻿using Application.DTOs;
using MediatR;

namespace Application.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(string Name) : IRequest<CategoryDto>;
}