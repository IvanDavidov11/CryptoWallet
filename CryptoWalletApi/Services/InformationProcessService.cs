using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using CryptoWalletApi.Interfaces;
using CryptoWalletApi.ViewModels;

namespace CryptoWalletApi.Services
{
    public class InformationProcessService
    {
        private CoinLoreApiManager _coinLoreApiManager = new();

        public async Task<CheckedCoinsDTO> ProcessCoinFileAsync(IFormFile file)
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

        public async Task<decimal> CalculateInitialPortfolioValueAsync(DatabaseManager dbManger)
        {
            var allCoins = await dbManger.GetOwnedCoinsAsync();

            decimal initialValue = 0;

            foreach (var coin in allCoins)
            {
                initialValue += (coin.Amount * coin.BuyPrice);
            }

            return initialValue;
        }

        public async Task<string> CalculateValuePercentageChangeOfCoinAsync(CoinViewModel coin)
        {
            CoinLoreCoinDTO coinLoreDtoValues = await _coinLoreApiManager.GetCoinInformationFromApiAsync(coin.CoinLoreId);
            decimal coinCurrentValue = coinLoreDtoValues.PriceUsd;
            decimal coinInitialValue = coin.BuyPrice;

            if (coinInitialValue == 0) throw new ArgumentException("Initial price cannot be zero.");

            decimal change = (coinCurrentValue - coinInitialValue) / coinInitialValue;
            return $"{change * 100}%";
        }

        public async Task<Dictionary<int, string>> CalculateValuePercentageChangeOfCoinsAsync(IEnumerable<CoinViewModel> coins)
        {
            ICollection<CoinLoreCoinDTO> coinDTOs = await _coinLoreApiManager
                .GetCoinsInformationFromAPI(coins
                    .Select(coin => coin.CoinLoreId)
                    .ToList());

            Dictionary<int, string> percentageChanges = new();
            foreach (var coinDTO in coinDTOs)
            {
                var matchedCoin = coins.FirstOrDefault(coin => coin.CoinLoreId.Equals(coinDTO.Id));

                if (matchedCoin is null)
                    continue; // handle fail of coin match

                decimal coinCurrentValue = coinDTO.PriceUsd;
                decimal coinInitialValue = matchedCoin.BuyPrice;
                decimal change = (coinCurrentValue - coinInitialValue) / coinInitialValue;
                string percentage = $"{(change * 100):F2}%"; // format to second decimal of percent

                percentageChanges.Add(matchedCoin.Id, percentage);
            }

            return percentageChanges;
        }
    }
}