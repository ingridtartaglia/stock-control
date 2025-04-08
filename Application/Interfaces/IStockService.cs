using Application.DTOs;

namespace Application.Interfaces {
    public interface IStockService {
        Task<StockMovementDTO> AddMovementAsync(StockMovementDTO movementDto);
        Task<IEnumerable<StockReportDTO>> GetStockReportAsync(DateTime date, string productCode);
    }
}