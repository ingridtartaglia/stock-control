using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IStockService
    {
        Task<StockMovementDTO> AddMovementAsync(StockMovementDTO movementDto);
        Task<IEnumerable<StockReportDTO>> GetStockReportAsync(DateTime date, string productCode = null);
    }
}