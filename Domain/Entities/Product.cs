namespace Domain.Entities {
    public class Product {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Code { get; set; }
    }
}