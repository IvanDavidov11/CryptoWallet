﻿using CryptoWalletApi.Data.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<CoinDatabaseModel> Coins { get; set; }

        public DbSet<UserPreferencesDatabaseModel> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoinDatabaseModel>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(DataConstants.CoinNameLengthMaximum);

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasPrecision(DataConstants.DecimalPrecision_TotalDigits, DataConstants.DecimalPrecision_DecimalPlaces);

                entity.Property(e => e.BuyPrice)
                    .IsRequired()
                    .HasPrecision(DataConstants.DecimalPrecision_TotalDigits, DataConstants.DecimalPrecision_DecimalPlaces);
            });
        }
    }
}