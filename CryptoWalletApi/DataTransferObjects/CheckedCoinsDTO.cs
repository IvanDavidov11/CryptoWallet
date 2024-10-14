using CryptoWalletApi.Data.DbModels;

namespace CryptoWalletApi.DataTransferObjects
{
    public class CheckedCoinsDTO
    {
        public ICollection<CoinModel> GoodCoins { get; set; }
        public ICollection<CoinModel> BadCoins { get; set; }

        public CheckedCoinsDTO(ICollection<CoinModel> goodCoins, ICollection<CoinModel> badCoins)
        {
            GoodCoins = goodCoins;
            BadCoins = badCoins;
        }
    }
}