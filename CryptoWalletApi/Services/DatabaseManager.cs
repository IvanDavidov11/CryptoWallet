﻿using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            _logger.LogInformation("Attempting to get all owned coins in database...");
            var coins = await _dbContext.Coins.ToListAsync();

            if (coins is null || !coins.Any())
            {
                _logger.LogWarning("No coins found in database. Please check if database is initialized properly.");
                return new List<CoinDatabaseModel>();
            }

            string successMessage = $"{coins.Count} Coins fetched successfully: ({string.Join(", ", coins.Select(coin => coin.ToString()))}).";
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