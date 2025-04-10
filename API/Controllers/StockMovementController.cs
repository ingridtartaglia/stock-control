using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [ApiController]
    [Route("api/stock")]
    public class StockMovementController : ControllerBase {
        private readonly IStockService _stockService;

        public StockMovementController(IStockService stockService) {
            _stockService = stockService;
        }

        [HttpPost("movements")]
        public async Task<ActionResult<StockMovementDTO>> AddMovement([FromBody] StockMovementDTO? movementDto) {
            if (movementDto == null) {
                return BadRequest("Movement data is required");
            }

            try {
                var result = await _stockService.AddMovementAsync(movementDto);
                return Ok(result);
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            } catch (InvalidOperationException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<StockReportDTO>>> GetStockReport([FromQuery] DateTime date, [FromQuery] string? productCode = null) {
            var report = await _stockService.GetStockReportAsync(date, productCode);
            return Ok(report);
        }
    }
}