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
        [Required]
        public required string CoinLoreId { get; set; }
    }
}