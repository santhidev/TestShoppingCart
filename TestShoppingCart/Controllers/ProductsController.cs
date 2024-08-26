using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestShoppingCart.DTOs;
using TestShoppingCart.Interfaces;
using TestShoppingCart.Models;

namespace TestShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.Stock?.Quantity ?? 0 // ดึงจำนวนสต็อกจาก Stock หากไม่มีสต็อกจะให้เป็น 0
            });

            return Ok(productDtos);
        }
    }
}
