using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs {
    public class StockMovementDTO {
        public required string ProductCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MovementType Type { get; set; }

        public int Quantity { get; set; }
    }
}