﻿using CryptoWalletApi.Data;
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

        public bool DbHasCoins()
        {
            return _dbContext.Coins.Count() > 0;
        }

        public async Task<bool> SeedDbWithCoinsFileAsync(ICollection<CoinDatabaseModel> coinModels)
        {
            try
            {
                await _dbContext.AddRangeAsync(coinModels);
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