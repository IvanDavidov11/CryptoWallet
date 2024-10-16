using CryptoWalletApi.Data.DbModels;

namespace CryptoWalletApi.DataTransferObjects
{
    public class CheckedCoinsDTO
    {
        public ICollection<CoinDatabaseModel> GoodCoins { get; set; }
        public ICollection<CoinDatabaseModel> BadCoins { get; set; }

        public CheckedCoinsDTO(ICollection<CoinDatabaseModel> goodCoins = null, ICollection<CoinDatabaseModel> badCoins = null)
        {
            GoodCoins = goodCoins ?? new List<CoinDatabaseModel>();
            BadCoins = badCoins ?? new List<CoinDatabaseModel>();
        }
    }
}