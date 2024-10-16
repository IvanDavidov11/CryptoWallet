using CryptoWalletApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal Amount { get; set; } = 0;

        [Required]
        public decimal BuyPrice { get; set; } = 0;

        // is used to get data from coinlore api.
        public string CoinLoreId { get; set; } = string.Empty;

        [NotMapped]
        public bool IsValid { get; set; } = true;

        public override string ToString()
        {
            return $"Coin Name: {Name}, Amount: {Amount}, Buy price: {BuyPrice}";
        }
    }
}