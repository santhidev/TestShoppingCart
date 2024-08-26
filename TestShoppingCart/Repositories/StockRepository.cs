using Microsoft.EntityFrameworkCore;
using TestShoppingCart.Data;
using TestShoppingCart.Interfaces;
using TestShoppingCart.Models;

namespace TestShoppingCart.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ShoppingCartContext _context;

        public StockRepository(ShoppingCartContext context)
        {
            _context = context;
        }

        public async Task<Stock> GetStockByProductIdAsync(int productId)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);
        }

        public async Task UpdateStockAsync(Stock stock)
        {
            _context.Entry(stock).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
