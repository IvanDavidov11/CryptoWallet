using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using System.Runtime.CompilerServices;

namespace CryptoWalletApi.Services
{
    public class InformationProcessService
    {
        private CoinLoreApiManager _coinLoreApiManager = new();

        public async Task<CheckedCoinsDTO> ProcessCoinFile(IFormFile file)
        {
            List<CoinModel> allCoins = await FileReaderAndParser.MapFileToCoinDbModelsAsync(file);
            ICollection<CoinModel> goodCoins = new List<CoinModel>();
            ICollection<CoinModel> badCoins = new List<CoinModel>();
            var checkedCoins = new CheckedCoinsDTO(goodCoins, badCoins);


            if (allCoins == null || allCoins.Count == 0)
                return checkedCoins;

            Dictionary<CoinModel, bool> result = await _coinLoreApiManager.TryFindCoinsInApiAsync(allCoins);
            foreach (var coin in result)
            {
                if (coin.Value == true)
                {
                    checkedCoins.GoodCoins.Add(coin.Key);
                }
                else
                {
                    checkedCoins.BadCoins.Add(coin.Key);
                }
            }

            return checkedCoins;
        }

        public async Task<decimal> CalculateInitialPortfolioValue(DatabaseManager dbManger)
        {
            var allCoins = await dbManger.GetOwnedCoinsAsync();

            decimal initialValue = 0;

            foreach (var coin in allCoins)
            {
                initialValue += (coin.Amount * coin.BuyPrice);
            }

            return initialValue;
        }

    }
}
