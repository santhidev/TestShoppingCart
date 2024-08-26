using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestShoppingCart.Helpers;
using TestShoppingCart.Interfaces;
using TestShoppingCart.Models;

namespace TestShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockRepository _stockRepository;

        public CartController(IProductRepository productRepository, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            return Ok(cart ?? new List<CartItem>());
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("The Name field is required.");
            }

            var product = await _productRepository.GetProductByIdAsync(item.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(i => i.ProductId == item.ProductId);
            var newQuantity = (existingItem?.Quantity ?? 0) + item.Quantity;

            if (newQuantity > product.Stock.Quantity)
            {
                return BadRequest("Cannot add more than available stock.");
            }

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Ok(cart);
        }

        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseQuantity([FromBody] CartItem item)
        {
            if (item == null || item.ProductId == 0 || string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("Invalid item or ProductId.");
            }

            var product = await _productRepository.GetProductByIdAsync(item.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == item.ProductId);

            // ตรวจสอบจำนวนสินค้าก่อนเพิ่ม
            if (existingItem != null)
            {
                if (existingItem.Quantity + 1 > product.Stock.Quantity)
                {
                    return BadRequest(new { message = "Cannot add more than available stock." });
                }
                existingItem.Quantity++;
            }
            else
            {
                if (1 > product.Stock.Quantity)
                {
                    return BadRequest(new { message = "Cannot add more than available stock." });
                }
                cart.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Ok(cart);
        }

        // Method สำหรับลดจำนวนสินค้า
        [HttpPost("decrease")]
        public IActionResult DecreaseQuantity([FromBody] CartItem item)
        {
            if (item == null || item.ProductId == 0 || string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("Invalid item or ProductId.");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null && existingItem.Quantity > 1)
            {
                existingItem.Quantity--;
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            else if (existingItem != null && existingItem.Quantity == 1)
            {
                cart.Remove(existingItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return Ok(cart);
        }

        [HttpPost("remove")]
        public IActionResult RemoveFromCart([FromBody] CartItem item)
        {
            if (item == null || item.ProductId == 0)  // ตรวจสอบเฉพาะ ProductId
            {
                return BadRequest("Invalid product ID.");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                cart.Remove(existingItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return Ok(cart);
        }

        [HttpPost("clear")]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return Ok();
        }

        // Endpoint สำหรับการ Checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || cart.Count == 0)
            {
                return BadRequest("Cart is empty.");
            }

            foreach (var item in cart)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    return NotFound($"Product not found for ProductId: {item.ProductId}");
                }

                if (product.Stock == null || product.Stock.Quantity < item.Quantity)
                {
                    return BadRequest($"Insufficient stock for product: {item.Name}");
                }

                // ตัดสต็อก
                product.Stock.Quantity -= item.Quantity;
            }

            try
            {
                // บันทึกการเปลี่ยนแปลง
                await _productRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // เพิ่มการ Log สำหรับ Debugging
                Console.WriteLine($"Error processing payment: {ex.Message}");
                return StatusCode(500, "Internal server error. Could not process payment.");
            }

            // ล้างตะกร้าสินค้า
            HttpContext.Session.Remove("Cart");

            return Ok();
        }


    }
}
