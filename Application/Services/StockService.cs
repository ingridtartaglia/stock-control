using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services {
    public class StockService : IStockService {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;

        public StockService(IProductRepository productRepository, IStockMovementRepository stockMovementRepository) {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<StockMovementDTO> AddMovementAsync(StockMovementDTO movementDto) {
            var product = await _productRepository.GetByCodeAsync(movementDto.ProductCode);
            if (product == null)
                throw new ArgumentException("Produto não encontrado");

            var currentStock = await _stockMovementRepository.GetCurrentStockAsync(product.Id);

            if (movementDto.Type == MovementType.Out && currentStock < movementDto.Quantity)
                throw new InvalidOperationException("Quantidade insuficiente em estoque");

            var movement = new StockMovement {
                Product = product,
                ProductId = product.Id,
                Type = movementDto.Type,
                Quantity = movementDto.Quantity,
                CreatedAt = DateTime.Now
            };

            await _stockMovementRepository.AddAsync(movement);
            return movementDto;
        }

        public async Task<IEnumerable<StockReportDTO>> GetStockReportAsync(DateTime date, string productCode) {
            var movements = await _stockMovementRepository.GetMovementsByDateAsync(date, productCode);
            var products = await _productRepository.GetAllAsync();

            var report = from p in products
                         join m in movements on p.Id equals m.ProductId into productMovements
                         select new StockReportDTO {
                             ProductName = p.Name,
                             ProductCode = p.Code,
                             TotalIn = productMovements.Where(m => m.Type == MovementType.In).Sum(m => m.Quantity),
                             TotalOut = productMovements.Where(m => m.Type == MovementType.Out).Sum(m => m.Quantity),
                             Balance = productMovements.Where(m => m.Type == MovementType.In).Sum(m => m.Quantity) -
                                     productMovements.Where(m => m.Type == MovementType.Out).Sum(m => m.Quantity)
                         };

            return report.ToList();
        }
    }
}