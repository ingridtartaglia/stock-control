using Domain.Entities;

namespace Domain.Interfaces {
    public interface IStockMovementRepository {
        Task<StockMovement> AddAsync(StockMovement movement);
        Task<IEnumerable<StockMovement>> GetMovementsByDateAsync(DateTime date, string? productCode = null);
        Task<int> GetCurrentStockAsync(Guid productId);
    }
}