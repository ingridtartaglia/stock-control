using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Test {
    public class ProductTests {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts() {
            // Arrange
            var options = new DbContextOptionsBuilder<StockControlContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            using (var context = new StockControlContext(options)) {
                context.Products.AddRange(new List<Product> {
                    new Product { Name = "Notebook", Code = "NB001" },
                    new Product { Name = "Mouse", Code = "MS001" },
                    new Product { Name = "Keyboard", Code = "KB001" }
                });
                context.SaveChanges();
            }

            using (var context = new StockControlContext(options)) {
                var repository = new ProductRepository(context);

                // Act
                var products = await repository.GetAllAsync();

                // Assert
                Assert.NotNull(products);
                Assert.Equal(3, products.Count());
                Assert.Contains(products, p => p.Name == "Notebook");
                Assert.Contains(products, p => p.Name == "Mouse");
                Assert.Contains(products, p => p.Name == "Keyboard");
            }
        }
    }
}
