namespace Domain.Entities {
    public class StockMovement {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public required Product Product { get; set; }
        public MovementType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
    }

    public enum MovementType {
        In,
        Out
    }
}