using TestShoppingCart.Models;

namespace TestShoppingCart.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock> GetStockByProductIdAsync(int productId);
        Task UpdateStockAsync(Stock stock);
    }
}
