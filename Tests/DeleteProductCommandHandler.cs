using Application.Products.Commands.DeleteProduct;
using Application.Interfaces;
using Application.Common;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Products
{
    [TestFixture]
    public class DeleteProductCommandHandlerTests
    {
        private Mock<IProductRepository> _repoMock = null!;
        private DeleteProductCommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_repoMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenProductNotFound()
        {
            // Arrange
            var command = new DeleteProductCommand(1);  // Command to delete a product with ID 1
            _repoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Product)null);  // Product does not exist

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.Success);  // Ensure it's a failure result
            Assert.AreEqual("Product not found.", result.Message);  // Ensure the correct error message
            _repoMock.Verify(r => r.Delete(It.IsAny<Product>()), Times.Never);  // Ensure Delete was not called
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);  // Ensure SaveChangesAsync was not called
        }

        [Test]
        public async Task Handle_ShouldDeleteProduct_AndReturnSuccess_WhenProductExists()
        {
            // Arrange
            var command = new DeleteProductCommand(1);  // Command to delete a product with ID 1
            var product = new Product { Id = 1, Name = "TestProduct", Price = 50, Stock = 10, CategoryId = 2 };  // Existing product with ID 1
            _repoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(product);  // Return existing product
            _repoMock.Setup(r => r.Delete(It.IsAny<Product>()));  // Mock the Delete method
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);  // Mock SaveChangesAsync

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Success);  // Ensure it's a success result
            Assert.AreEqual(true, result.Data);  // Ensure the returned value is true (success)
            _repoMock.Verify(r => r.Delete(It.Is<Product>(p => p.Id == command.Id)), Times.Once);  // Ensure Delete was called once with the correct product
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);  // Ensure SaveChangesAsync was called once
        }
    }
}
