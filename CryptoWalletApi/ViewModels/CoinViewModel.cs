using CryptoWalletApi.Data;
using CryptoWalletApi.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletApi.ViewModels
{
    public class CoinViewModel : ICoin
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

        [Required] // is used to get data from coinlore api.
        public string CoinLoreId { get; set; }

        public string? PercentageChange { get; set; }

        public decimal? CurrentPrice { get; set; }
    }
}