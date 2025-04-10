using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class ProductRepository : IProductRepository {
        private readonly StockControlContext _context;

        public ProductRepository(StockControlContext context) {
            _context = context;
        }

        public async Task<Product> GetByCodeAsync(string code) {
            return await _context.Products.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() {
            return await _context.Products.ToListAsync();
        }
    }
}