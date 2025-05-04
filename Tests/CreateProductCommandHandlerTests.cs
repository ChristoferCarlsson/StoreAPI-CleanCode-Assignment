using Application.Products.Commands.CreateProduct;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Products
{
    [TestFixture]
    public class CreateProductCommandHandlerTests
    {
        private Mock<IProductRepository> _repoMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private CreateProductCommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateProductCommandHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenProductAlreadyExists()
        {
            // Arrange
            var command = new CreateProductCommand("TestProduct", 100, 10, 1);  // Product data
            var existingProduct = new Product { Name = command.Name, Price = 100, Stock = 10, CategoryId = 1 };  // Product exists with this name and category

            _repoMock.Setup(r => r.GetByNameAndCategoryAsync(command.Name, command.CategoryId))
                     .ReturnsAsync(existingProduct);  // Product exists, return it

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.Success);  // Ensure it's a failure result
            Assert.AreEqual("Product with the same name and category already exists.", result.Message);  // Adjusted to check for the "Message" property
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);  // Ensure AddAsync was NOT called
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);  // Ensure SaveChangesAsync was NOT called
        }

        [Test]
        public async Task Handle_ShouldAddProduct_AndReturnDto_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new CreateProductCommand("NewProduct", 50, 20, 2);  // New product data
            var category = new Category { Id = 2, Name = "Category 2" };  // Category with Id 2
            var newProduct = new Product { Name = command.Name, Price = command.Price, Stock = command.Stock, CategoryId = command.CategoryId };
            var productDto = new ProductDto { Name = command.Name, Price = command.Price, Stock = command.Stock, CategoryId = command.CategoryId };

            _repoMock.Setup(r => r.GetByNameAndCategoryAsync(command.Name, command.CategoryId))
                     .ReturnsAsync((Product)null);  // Product does not exist, return null
            _mapperMock.Setup(m => m.Map<Product>(It.IsAny<CreateProductCommand>()))
                       .Returns(newProduct);  // Map the command to a Product
            _mapperMock.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                       .Returns(productDto);  // Map the product to a ProductDto

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Success);  // Ensure the result is a success
            Assert.AreEqual(command.Name, result.Data.Name);  // Ensure the returned ProductDto's name matches the command's name
            _repoMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == command.Name)), Times.Once);  // Ensure AddAsync was called once with the correct product
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);  // Ensure SaveChangesAsync was called once
        }
    }
}
