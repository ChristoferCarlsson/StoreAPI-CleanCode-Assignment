using MediatR;
using Application.Common;

namespace Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest<OperationResult<bool>>;
