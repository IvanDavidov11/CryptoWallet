using System.ComponentModel.DataAnnotations;

namespace CryptoWalletApi.ViewModels
{
    public class CoinsViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal BuyPrice { get; set; }
    }
}
