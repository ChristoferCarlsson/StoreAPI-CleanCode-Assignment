using Application.Common;
using Application.Interfaces;
using MediatR;

namespace Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, OperationResult<bool>>
{
    private readonly IProductRepository _repo;

    public DeleteProductCommandHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<OperationResult<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product == null)
        {
            return OperationResult<bool>.FailureResult("Product not found.");
        }

        _repo.Delete(product);
        await _repo.SaveChangesAsync();

        return OperationResult<bool>.SuccessResult(true);
    }
}
