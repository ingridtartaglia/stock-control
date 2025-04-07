using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockMovementController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockMovementController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("movement")]
        public async Task<ActionResult<StockMovementDTO>> AddMovement([FromBody] StockMovementDTO movementDto)
        {
            try
            {
                var result = await _stockService.AddMovementAsync(movementDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report")]
        public async Task<ActionResult<IEnumerable<StockReportDTO>>> GetStockReport([FromQuery] DateTime date, [FromQuery] string productCode = null)
        {
            var report = await _stockService.GetStockReportAsync(date, productCode);
            return Ok(report);
        }
    }
}