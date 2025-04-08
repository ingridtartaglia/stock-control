using Domain.Entities;

namespace Application.DTOs {
    public class StockMovementDTO {
        public required string ProductCode { get; set; }
        public MovementType Type { get; set; }
        public int Quantity { get; set; }
    }
}