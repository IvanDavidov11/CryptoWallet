using CryptoWalletApi.Data.DbModels;

namespace CryptoWalletApi.DataTransferObjects
{
    public class CheckedCoinsDTO
    {
        public ICollection<CoinDatabaseModel> GoodCoins { get; set; }
        public ICollection<CoinDatabaseModel> BadCoins { get; set; }

        public CheckedCoinsDTO(ICollection<CoinDatabaseModel> goodCoins, ICollection<CoinDatabaseModel> badCoins)
        {
            GoodCoins = goodCoins;
            BadCoins = badCoins;
        }
    }
}