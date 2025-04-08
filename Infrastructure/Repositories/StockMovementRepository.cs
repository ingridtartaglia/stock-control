using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class StockMovementRepository : IStockMovementRepository {
        private readonly StockControlContext _context;

        public StockMovementRepository(StockControlContext context) {
            _context = context;
        }

        public async Task<StockMovement> AddAsync(StockMovement movement) {
            await _context.StockMovements.AddAsync(movement);
            await _context.SaveChangesAsync();
            return movement;
        }

        public async Task<IEnumerable<StockMovement>> GetMovementsByDateAsync(DateTime date, string productCode) {
            var query = _context.StockMovements
                .Include(sm => sm.Product)
                .Where(sm => sm.CreatedAt.Date == date.Date);

            if (!string.IsNullOrEmpty(productCode)) {
                query = query.Where(sm => sm.Product.Code == productCode);
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetCurrentStockAsync(Guid productId) {
            var movements = await _context.StockMovements
                .Where(sm => sm.ProductId == productId)
                .ToListAsync();

            var entries = movements.Where(m => m.Type == MovementType.In).Sum(m => m.Quantity);
            var exits = movements.Where(m => m.Type == MovementType.Out).Sum(m => m.Quantity);

            return entries - exits;
        }
    }
}