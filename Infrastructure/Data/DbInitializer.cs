using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(StockControlContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }

            var products = new Product[]
            {
                new Product { Name = "Notebook", Code = "NB001" },
                new Product { Name = "Mouse", Code = "MS001" },
                new Product { Name = "Teclado", Code = "TK001" },
                new Product { Name = "Monitor", Code = "MN001" },
                new Product { Name = "Headphone", Code = "HP001" }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}