using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.Products.Queries.GetProductById;
using AutoMapper;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Products
{
    [TestFixture]
    public class GetProductByIdQueryHandlerTests
    {
        private Mock<IProductRepository> _repoMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private GetProductByIdQueryHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductByIdQueryHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ProductExists_ReturnsMappedDto()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 100, Stock = 5, CategoryId = 2 };
            var dto = new ProductDto { Id = 1, Name = "Test Product", Price = 100, Stock = 5, CategoryId = 2 };

            _repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(dto);

            var query = new GetProductByIdQuery(product.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(dto.Id, result.Data.Id);
            Assert.AreEqual(dto.Name, result.Data.Name);
        }

        [Test]
        public async Task Handle_ProductNotFound_ReturnsNullData()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);
            var query = new GetProductByIdQuery(999);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNull(result.Data);
        }
    }
}
