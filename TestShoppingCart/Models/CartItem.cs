using System.ComponentModel.DataAnnotations;

namespace TestShoppingCart.Models
{
    public class CartItem
    {
        [Required]
        public int ProductId { get; set; }

        // ลบการตรวจสอบ [Required] ออกจาก Name หากไม่ต้องการให้จำเป็น
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
