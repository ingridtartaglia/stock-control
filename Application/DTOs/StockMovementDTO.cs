using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.DTOs {
    public class StockMovementDTO {
        public required string ProductCode { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MovementType Type { get; set; }
        
        public int Quantity { get; set; }
    }
}