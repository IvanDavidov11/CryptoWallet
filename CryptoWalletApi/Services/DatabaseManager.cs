using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace CryptoWalletApi.Services
{
    public class DatabaseManager
    {
        private DatabaseContext _dbContext;
        private ILogger _logger;

        public DatabaseManager(DatabaseContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ICollection<CoinDatabaseModel>> GetOwnedCoinsAsync()
        {
            _logger.LogInformation("| | | Attempting to get all owned coins in database...");
            var coins = await _dbContext.Coins.ToListAsync();

            if (coins is null || !coins.Any())
            {
                _logger.LogWarning("| | | No coins found in database. Please check if database is initialized properly.");
                return new List<CoinDatabaseModel>();
            }

            string successMessage = $"| | | {coins.Count} Coins fetched successfully: ({string.Join(", ", coins.Select(coin => coin.ToString()))}).";
            _logger.LogInformation(successMessage);
            return coins;
        }

        public async Task<UserPreferencesDatabaseModel> GetUserPreferencesAsync()
        {
            if (!DbHasUserPreferences())
                await AddInitialUserPreferencesAsync();

            var userPreferences = await _dbContext.UserPreferences.FirstOrDefaultAsync(); // log error for null return
            // Log an error for null return, if needed

            return userPreferences;
        }

        public bool DbHasCoins()
        {
            _logger.LogInformation("Attempting to check if Database has any saved coins in the coins table...");
            
            if (!_dbContext.Coins.Any())
            {
                _logger.LogWarning("Database has no saved coins in coin table, returning false.");
                return false;
            }

            _logger.LogInformation("Database has saved coins in coin table, returning true.");
            return true;
        }

        public bool DbHasUserPreferences()
        {
            _logger.LogInformation("Attempting to check if Database has entry in UserPreferences table...");
            if (!_dbContext.UserPreferences.Any())
            {
                _logger.LogWarning("UserPreferences not found in database, returning false.");
                return false;
            }

            _logger.LogInformation("UserPreferences found in database, returning true.");
            return true;
        }

        public async Task<bool> SeedDbWithCoinsFileAsync(ICollection<CoinDatabaseModel> coinModels)
        {
            try
            {
                _logger.LogInformation($"Attempting to add {coinModels.Count} coins to database...");
                await _dbContext.Coins.AddRangeAsync(coinModels);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Added coins to database successfully.");

                await AddInitialUserPreferencesAsync();

                _logger.LogInformation($"Database is seeded correctly.");
                return true;
            }
            catch
            {
                _logger.LogError($"Failed to add coins to database.");
                return false;
            }
        }

        public async Task<bool> AddInitialUserPreferencesAsync()
        {
            if (DbHasUserPreferences())
                return false;

            _logger.LogInformation("Initial user preferences are missing. Attempting to add them to the database...");
            var userPreferences = new UserPreferencesDatabaseModel();
            try
            {
                await _dbContext.UserPreferences.AddAsync(userPreferences);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("UserPreferences successfully added to database.");

                return true;
            }
            catch
            {
                _logger.LogWarning("Failed to add UserPreferences to database.");
                return false;
            }
        }

        public async Task<bool> UpdateRefreshIntervalOfUser(int refreshInterval)
        {
            try
            {
                var userPreferences = await GetUserPreferencesAsync();
                userPreferences.RefreshInterval = refreshInterval;
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