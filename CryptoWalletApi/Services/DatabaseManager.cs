using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
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

        public async Task<ICollection<CoinDatabaseModel>> GetOwnedCoinsAsync()
        {
            return await _dbContext.Coins.ToListAsync();
        }

        public async Task<UserPreferencesDatabaseModel> GetUserPreferencesAsync()
        {
            var userPreferences = await _dbContext.UserPreferences.FirstOrDefaultAsync(); // log error for null return
            if (userPreferences == null)
            {
                // Log an error for null return, if needed
            }

            return userPreferences;
        }

        public bool DbHasCoins()
        {
            return _dbContext.Coins.Count() > 0;
        }

        public bool DbHasUserPreferences()
        {
            return _dbContext.UserPreferences.Count() > 0;
        }

        public async Task<bool> SeedDbWithCoinsFileAsync(ICollection<CoinDatabaseModel> coinModels)
        {
            try
            {
                await _dbContext.Coins.AddRangeAsync(coinModels);
                await _dbContext.SaveChangesAsync();
                await AddInitialUserPreferencesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddInitialUserPreferencesAsync()
        {
            if (DbHasUserPreferences())
                return false;

            var userPreferences = new UserPreferencesDatabaseModel();
            try
            {
                await _dbContext.UserPreferences.AddAsync(userPreferences);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ClearCoinsFromDbAsync()
        {
            _dbContext.Coins.RemoveRange(_dbContext.Coins);
            await _dbContext.SaveChangesAsync();

            return !DbHasCoins(); // if db has no coins, the removal was successful
        }
    }
}