using CryptoWalletApi.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletApi.Data.DbModels
{
    public class CoinModel : ICoin
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CoinNameLengthMaximum)]
        public string Name { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal BuyPrice { get; set; }
    }
}