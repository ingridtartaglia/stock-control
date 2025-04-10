using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }
    }
}