using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Test {
    public class StockTests {
        [Fact]
        public async Task GetStockReportAsync_ShouldReturnReports_WhenOnlyDateIsProvided() {
            // Arrange
            var date = new DateTime(2025, 4, 9);
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            var product1 = new Product { Id = Guid.NewGuid(), Name = "Notebook", Code = "NB001" };
            var product2 = new Product { Id = Guid.NewGuid(), Name = "Mouse", Code = "MS001" };

            mockProductRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Product> { product1, product2 });

            mockStockMovementRepository.Setup(repo => repo.GetMovementsByDateAsync(date, null))
                .ReturnsAsync(new List<StockMovement> {
                        new StockMovement { ProductId = product1.Id, Product = product1, Type = MovementType.In, Quantity = 10, CreatedAt = date },
                        new StockMovement { ProductId = product2.Id, Product = product2, Type = MovementType.Out, Quantity = 5, CreatedAt = date }
                });

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            // Act
            var reports = await service.GetStockReportAsync(date);

            // Assert
            Assert.NotNull(reports);
            Assert.NotEmpty(reports);
        }

        [Fact]
        public async Task GetStockReportAsync_ShouldReturnReports_WhenDateAndProductCodeAreProvided() {
            // Arrange
            var date = new DateTime(2025, 4, 9);
            var productCode = "NB001";
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            var product = new Product { Id = Guid.NewGuid(), Name = "Notebook", Code = productCode };

            mockProductRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Product> { product });

            mockStockMovementRepository.Setup(repo => repo.GetMovementsByDateAsync(date, productCode))
                .ReturnsAsync(new List<StockMovement> {
                        new StockMovement { ProductId = product.Id, Product = product, Type = MovementType.In, Quantity = 10, CreatedAt = date },
                        new StockMovement { ProductId = product.Id, Product = product, Type = MovementType.Out, Quantity = 5, CreatedAt = date }
                });

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            // Act
            var reports = await service.GetStockReportAsync(date, productCode);

            // Assert
            Assert.NotNull(reports);
            Assert.Single(reports);
            Assert.Equal(productCode, reports.First().ProductCode);
        }

        [Fact]
        public async Task AddMovementAsync_ShouldAddStockMovement_WhenProductCodeExists() {
            // Arrange
            var productCode = "NB001";
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            var product = new Product { Id = Guid.NewGuid(), Name = "Notebook", Code = productCode };

            mockProductRepository.Setup(repo => repo.GetByCodeAsync(productCode))
                .ReturnsAsync(product);

            mockStockMovementRepository.Setup(repo => repo.GetCurrentStockAsync(product.Id))
                .ReturnsAsync(10);

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            var movementDto = new StockMovementDTO {
                ProductCode = productCode,
                Type = MovementType.In,
                Quantity = 5
            };

            // Act
            var result = await service.AddMovementAsync(movementDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productCode, result.ProductCode);
            Assert.Equal(MovementType.In, result.Type);
            Assert.Equal(5, result.Quantity);
        }

        [Fact]
        public async Task AddMovementAsync_ShouldThrowError_WhenProductCodeDoesNotExist() {
            // Arrange
            var productCode = "NEW001";
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            mockProductRepository.Setup(repo => repo.GetByCodeAsync(productCode))
                .ReturnsAsync((Product)null);

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            var movementDto = new StockMovementDTO {
                ProductCode = productCode,
                Type = MovementType.In,
                Quantity = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMovementAsync(movementDto));
            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task AddMovementAsync_ShouldThrowError_WhenProductCodeIsNull() {
            // Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            var movementDto = new StockMovementDTO {
                ProductCode = null,
                Type = MovementType.In,
                Quantity = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMovementAsync(movementDto));
            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task AddMovementAsync_ShouldThrowError_WhenQuantityIsZero() {
            // Arrange
            var productCode = "NB001";
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            var product = new Product { Id = Guid.NewGuid(), Name = "Notebook", Code = productCode };

            mockProductRepository.Setup(repo => repo.GetByCodeAsync(productCode))
                .ReturnsAsync(product);

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            var movementDto = new StockMovementDTO {
                ProductCode = productCode,
                Type = MovementType.In,
                Quantity = 0
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddMovementAsync(movementDto));
            Assert.Equal("Quantity must be greater than zero", exception.Message);
        }

        [Fact]
        public async Task AddMovementAsync_ShouldThrowError_WhenQuantityExceedsCurrentStock() {
            // Arrange
            var productCode = "NB001";
            var mockProductRepository = new Mock<IProductRepository>();
            var mockStockMovementRepository = new Mock<IStockMovementRepository>();

            var product = new Product { Id = Guid.NewGuid(), Name = "Notebook", Code = productCode };

            mockProductRepository.Setup(repo => repo.GetByCodeAsync(productCode))
                .ReturnsAsync(product);

            // Simula que o estoque atual é 5
            mockStockMovementRepository.Setup(repo => repo.GetCurrentStockAsync(product.Id))
                .ReturnsAsync(5);

            var service = new StockService(mockProductRepository.Object, mockStockMovementRepository.Object);

            var movementDto = new StockMovementDTO {
                ProductCode = productCode,
                Type = MovementType.Out,
                Quantity = 10
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddMovementAsync(movementDto));
            Assert.Equal("Insufficient quantity in stock", exception.Message);
        }

    }
}
