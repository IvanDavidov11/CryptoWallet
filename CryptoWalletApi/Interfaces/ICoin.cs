using System.ComponentModel.DataAnnotations;

namespace CryptoWalletApi.Interfaces
{
    public interface ICoin
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal BuyPrice { get; set; }

        public string CoinLoreId { get; set; }
    }
}
