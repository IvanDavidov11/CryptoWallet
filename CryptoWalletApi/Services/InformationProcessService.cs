using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using CryptoWalletApi.ViewModels;

namespace CryptoWalletApi.Services
{
    public class InformationProcessService
    {
        private CoinLoreApiManager _coinLoreApiManager = new();

        public async Task<CheckedCoinsDTO> CheckValidityOfCoinFile(IFormFile file)
        {
            List<CoinDatabaseModel> allCoins = await FileReaderAndParser.MapFileToCoinDbModelsAsync(file);
            List<CoinDatabaseModel> goodCoins = new();
            List<CoinDatabaseModel> badCoins = new();
            var checkedCoins = new CheckedCoinsDTO(goodCoins, badCoins);

            if (allCoins == null || allCoins.Count == 0)
                return checkedCoins;

            Dictionary<CoinDatabaseModel, bool> result = await _coinLoreApiManager
                 .VerifyAgainstAllCoinsInApi(allCoins);

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

        public decimal CalculateInitialPortfolioValue(IEnumerable<CoinDatabaseModel> allCoins)
        {
            decimal initialValue = 0;

            foreach (var coin in allCoins)
                initialValue += (coin.Amount * coin.BuyPrice);

            return initialValue;
        }

        public async Task<decimal> CalculateCurrentPortfolioValueAsync(List<CoinDatabaseModel> allCoins)
        {
            var allCoinPrices = await CalculateCurrentPriceOfCoinAsync(allCoins);
            decimal currentValue = 0;

            foreach (var coin in allCoins)
                currentValue += (coin.Amount * allCoinPrices[coin.Id]);

            return currentValue;
        }

        public Dictionary<int,string> CalculateValuePercentageChangeOfCoins(List<CoinViewModel> coins)
        {
            Dictionary<int,string> currentPrices = new();

            foreach (var coin in coins)
            {
                if (coin.CurrentPrice is null)
                    continue;

                decimal coinCurrentValue = coin.CurrentPrice.GetValueOrDefault();
                decimal coinInitialValue = coin.BuyPrice;
                decimal change = (coinCurrentValue - coinInitialValue) / coinInitialValue;
                string percentage = $"{(change * 100):F2}%"; // format to second decimal of percent
                currentPrices.Add(coin.Id, percentage);
            }

            return currentPrices;
        }

        public async Task<Dictionary<int, decimal>> CalculateCurrentPriceOfCoinAsync(List<CoinViewModel> coins)
        {
            ICollection<CoinLoreCoinDTO> coinDTOs = await _coinLoreApiManager
                .GetCoinsInformationFromApiAsync(coins
                    .Select(coin => coin.CoinLoreId)
                    .ToList());

            Dictionary<int, decimal> coinPrices = new();
            foreach (var coinDTO in coinDTOs)
            {
                var matchedCoin = coins.FirstOrDefault(coin => coin.CoinLoreId.Equals(coinDTO.Id));
                
                if (matchedCoin is null)
                    continue; // handle fail of coin match
                
                decimal coinCurrentValue = coinDTO.PriceUsd;

                coinPrices.Add(matchedCoin.Id, coinCurrentValue);
            }

            return coinPrices;
        }

        public async Task<Dictionary<int, decimal>> CalculateCurrentPriceOfCoinAsync(List<CoinDatabaseModel> coins)
        {
            ICollection<CoinLoreCoinDTO> coinDTOs = await _coinLoreApiManager
                .GetCoinsInformationFromApiAsync(coins
                    .Select(coin => coin.CoinLoreId)
                    .ToList());

            Dictionary<int, decimal> coinPrices = new();
            foreach (var coinDTO in coinDTOs)
            {
                var matchedCoin = coins.FirstOrDefault(coin => coin.CoinLoreId.Equals(coinDTO.Id));

                if (matchedCoin is null)
                    continue; // handle fail of coin match

                decimal coinCurrentValue = coinDTO.PriceUsd;

                coinPrices.Add(matchedCoin.Id, coinCurrentValue);
            }

            return coinPrices;
        }
    }
}