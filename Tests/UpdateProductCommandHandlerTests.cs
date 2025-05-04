using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.Products.Commands.UpdateProduct;
using AutoMapper;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Products
{
    [TestFixture]
    public class UpdateProductCommandHandlerTests
    {
        private Mock<IProductRepository> _repoMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private UpdateProductCommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateProductCommandHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ProductExists_ReturnsUpdatedProductDto()
        {
            // Arrange
            var command = new UpdateProductCommand(1, "UpdatedName", 99.99m, 10, 2);
            var existingProduct = new Product
            {
                Id = 1,
                Name = "OldName",
                Price = 49.99m,
                Stock = 5,
                CategoryId = 1
            };

            var updatedDto = new ProductDto
            {
                Id = 1,
                Name = "UpdatedName",
                Price = 99.99m,
                Stock = 10,
                CategoryId = 2
            };

            _repoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(existingProduct);
            _mapperMock.Setup(m => m.Map<ProductDto>(It.IsAny<Product>())).Returns(updatedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("UpdatedName", result.Data.Name);
            Assert.AreEqual(99.99m, result.Data.Price);
            Assert.AreEqual(10, result.Data.Stock);
            Assert.AreEqual(2, result.Data.CategoryId);
            _repoMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ProductNotFound_ReturnsFailure()
        {
            // Arrange
            var command = new UpdateProductCommand(99, "Nonexistent", 10.0m, 1, 1);
            _repoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Product?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNull(result.Data);
            _repoMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
