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
                throw new ArgumentException("Product not found");

            if (movementDto.Quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero");

            var currentStock = await _stockMovementRepository.GetCurrentStockAsync(product.Id);

            if (movementDto.Type == MovementType.Out && currentStock < movementDto.Quantity)
                throw new InvalidOperationException("Insufficient quantity in stock");

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

        public async Task<IEnumerable<StockReportDTO>> GetStockReportAsync(DateTime date, string? productCode = null) {
            var movements = await _stockMovementRepository.GetMovementsByDateAsync(date, productCode);
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(productCode)) {
                products = products.Where(p => p.Code == productCode);
            }

            var report = products.Select(p => {
                var productMovements = movements.Where(m => m.ProductId == p.Id);
                var totalIn = productMovements.Where(m => m.Type == MovementType.In).Sum(m => m.Quantity);
                var totalOut = productMovements.Where(m => m.Type == MovementType.Out).Sum(m => m.Quantity);

                return new StockReportDTO {
                    ProductName = p.Name,
                    ProductCode = p.Code,
                    TotalIn = totalIn,
                    TotalOut = totalOut,
                    Balance = totalIn - totalOut
                };
            });

            return report.ToList();
        }
    }
}