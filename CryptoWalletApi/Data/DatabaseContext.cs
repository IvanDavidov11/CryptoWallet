using CryptoWalletApi.Data.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<CoinModel> Coins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoinModel>(entity =>
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
