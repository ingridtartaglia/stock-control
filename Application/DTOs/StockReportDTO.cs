namespace Application.DTOs {
    public class StockReportDTO {
        public required string ProductName { get; set; }
        public required string ProductCode { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int Balance { get; set; }
    }
}