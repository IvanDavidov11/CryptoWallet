using CryptoWalletApi.Data;
using CryptoWalletApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Services
{
    public class DatabaseManager
    {
        private DatabaseContext _dbContext;

        public DatabaseManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CoinViewModel>> GetOwnedCoinsAsync()
        {
            return await _dbContext.Coins.Select(coin => new CoinViewModel()
            {
                Id = coin.Id,
                Amount = coin.Amount,
                BuyPrice = coin.BuyPrice,
                Name = coin.Name,
            }).ToListAsync();
        }
    }
}
