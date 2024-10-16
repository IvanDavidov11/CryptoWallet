using CryptoWalletApi.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletApi.Data.DbModels
{
    public class CoinDatabaseModel : ICoin
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CoinNameLengthMaximum)]
        public required string Name { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal BuyPrice { get; set; }

        // is used to get data from coinlore api.
        public required string CoinLoreId { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Coin Name: {Name}, Amount: {Amount}, Buy price: {BuyPrice}";
        }
    }
}