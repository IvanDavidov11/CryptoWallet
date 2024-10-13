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

        public bool DbHasCoins()
        {
            return _dbContext.Coins.Count() > 0;
        }

        public async Task<bool> SeedDbWithCoins(IFormFile file)
        {
            List<Data.DbModels.CoinModel> allCoins =await FileReaderAndParser.MapFileToCoinDbModels(file);
            if (allCoins == null || allCoins.Count == 0)
                return false;

            await _dbContext.AddRangeAsync(allCoins);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCoinsFromDb()
        {
            _dbContext.Coins.RemoveRange(_dbContext.Coins);
            await _dbContext.SaveChangesAsync();

            return !DbHasCoins(); // if db has no coins, the removal was successful
        }
    }
}
