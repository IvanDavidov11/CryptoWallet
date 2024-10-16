using CryptoWalletApi.Controllers;
using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using CryptoWalletApi.ViewModels;

namespace CryptoWalletApi.Services
{
    public class InformationProcessService
    {
        private ILogger _logger;
        private CoinLoreApiManager _coinLoreApiManager;

        public InformationProcessService(ILogger<CoinsController> logger)
        {
            _logger = logger;
            _coinLoreApiManager = new(logger);
        }

        public async Task<CheckedCoinsDTO> CheckValidityOfCoinFile(IFormFile file)
        {
            List<CoinDatabaseModel> allCoins = await FileReaderAndParser.MapFileToCoinDbModelsAsync(file);
            List<CoinDatabaseModel> goodCoins = new();
            List<CoinDatabaseModel> badCoins = new();
            var checkedCoins = new CheckedCoinsDTO(goodCoins, badCoins);

            if (allCoins == null || allCoins.Count == 0)
                return checkedCoins;

            Dictionary<CoinDatabaseModel, bool> result = await VerifyCoinsWithCoinLoreApiAsync(allCoins);

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

        /// <summary>
        /// Loops through all the coins in CoinLoreApi, until it verifies all coinsToCheck, or it reaches the end.
        /// </summary>
        public async Task<Dictionary<CoinDatabaseModel, bool>> VerifyCoinsWithCoinLoreApiAsync(ICollection<CoinDatabaseModel> coinsToCheck)
        {
            int amountOfCoinsInCoinLore = (await _coinLoreApiManager.GetGlobalDataFromApiAsync()).CoinsCount;
            Dictionary<CoinDatabaseModel, bool> overallCheckedCoins = new();

            for (int index = 0; index < amountOfCoinsInCoinLore; index += 100)
            {
                List<CoinLoreCoinDTO> responseApiCoins = await _coinLoreApiManager.GetCoinsFromApiAsync(index);
                Dictionary<CoinDatabaseModel, bool> checkedCoins = VerifyCoinsAgainstApiResponse(coinsToCheck, responseApiCoins);
                IEnumerable<KeyValuePair<CoinDatabaseModel, bool>> verifiedCoins = checkedCoins.Where(dto => dto.Value == true);

                foreach (var verifiedCoin in verifiedCoins)
                {
                    overallCheckedCoins.Add(verifiedCoin.Key, verifiedCoin.Value);
                    coinsToCheck.Remove(verifiedCoin.Key);
                }

                if (!coinsToCheck.Any())
                    break;
            }

            if (coinsToCheck.Any())
            {
                foreach (var badCoin in coinsToCheck)
                    overallCheckedCoins.Add(badCoin, false);
            }

            return overallCheckedCoins;
        }

        /// <summary>
        /// Tries to match the name or symbol of coinsToCheck against apiResponseCoins.
        /// </summary>
        public Dictionary<CoinDatabaseModel, bool> VerifyCoinsAgainstApiResponse(
            ICollection<CoinDatabaseModel> coinsToCheck,
            ICollection<CoinLoreCoinDTO> apiResponseCoins)
        {
            Dictionary<string, ApiCoinWithNameAndSymbolDTO> apiResponseCoinWithNameAndSymbol = apiResponseCoins.ToDictionary(
                c => c.Id,
                c => new ApiCoinWithNameAndSymbolDTO(c.Name, c.Symbol),
                StringComparer.OrdinalIgnoreCase);

            var apiCoinDictionaryWithSymbol = apiResponseCoins.ToDictionary(c => c.Id, c => c.Symbol, StringComparer.OrdinalIgnoreCase);
            Dictionary<CoinDatabaseModel, bool> checkedCoins = new();

            foreach (var coin in coinsToCheck)
            {
                var coinName = coin.Name;
                bool coinFound = false;

                foreach (var apiCoin in apiResponseCoinWithNameAndSymbol)
                {
                    if (apiCoin.Value.Name == coinName || apiCoin.Value.Symbol == coinName)
                    {
                        coin.CoinLoreId = apiCoin.Key;
                        checkedCoins.Add(coin, true);
                        coinFound = true;
                        break;
                    }
                }

                if (!coinFound)
                    checkedCoins.Add(coin, false);
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

        public Dictionary<int, string> CalculateValuePercentageChangeOfCoins(List<CoinViewModel> coins)
        {
            _logger.LogInformation("Calculating coin percentage change since inital buy...");
            Dictionary<int, string> coinPercentageIncreases = new();
            int processedCount = 0;

            foreach (var coin in coins)
            {
                if (coin.CurrentPrice == 0)
                {
                    _logger.LogWarning($"Coin: {coin.Name} (ID: {coin.Id}) doesn't have a current price set. CurrentPrice: {coin.CurrentPrice}");
                    continue;
                }

                decimal coinCurrentValue = coin.CurrentPrice;
                decimal coinInitialValue = coin.BuyPrice;
                decimal change = (coinCurrentValue - coinInitialValue) / coinInitialValue;
                string percentage = $"{(change * 100):F2}%"; // format to second decimal of percent
                coinPercentageIncreases.Add(coin.Id, percentage);

                _logger.LogInformation($"Coin: {coin.Name} percentage change calculated as %{percentage}");
                processedCount++;
            }

            _logger.LogInformation($"Calculation of percentage change has finished. Processed {processedCount}/{coins.Count} coins.");
            return coinPercentageIncreases;
        }

        public async Task<Dictionary<int, decimal>> CalculateCurrentPriceOfCoinAsync(List<CoinViewModel> coins)
        {
            ICollection<CoinLoreCoinDTO> coinDTOs = await _coinLoreApiManager
                .GetCoinsInformationFromApiAsync(coins
                    .Select(coin => coin.CoinLoreId)
                    .ToList());

            if (coinDTOs.Count != coins.Count)
            {
                var warningMessage = "1 or more coins couldn't be matched. Check CoinLoreId property of CoinViewModels:" +
                    $"{string.Join(", ", coins.Select(coin => coin.CoinLoreId))}";

                _logger.LogWarning(warningMessage);
            }

            _logger.LogInformation("Calculating current prices of coins...");
            Dictionary<int, decimal> coinPrices = new();

            if (!coinDTOs.Any())
            {
                _logger.LogWarning("No coin information was retrieved from the API.");
                return coinPrices;
            }

            int processedCount = 0;
            foreach (var coinDTO in coinDTOs)
            {
                CoinViewModel? matchedCoin = coins.FirstOrDefault(coin => coin.CoinLoreId.Equals(coinDTO.Id));

                if (matchedCoin is null)
                {
                    _logger.LogWarning($"Coin:({coinDTO.ToString()}) couldn't be matched to CoinViewModel.CoinLoreId:" +
                        $"{string.Join(", ", coins.Select(coin => coin.CoinLoreId))}");
                    continue;
                }

                decimal coinCurrentValue = coinDTO.PriceUsd;

                if (coinDTO.PriceUsd == 0)
                    _logger.LogWarning($"Coin:({coinDTO.ToString()})'s price is null or 0, check CoinLoreApi's data.");

                coinPrices.Add(matchedCoin.Id, coinCurrentValue);

                _logger.LogInformation($"Coin:({coinDTO.ToString()})'s price has been set to ({coinDTO.PriceUsd}).");
                processedCount++;
            }

            _logger.LogInformation($"Calculation of current coin prices has finished. Processed {processedCount}/{coins.Count} coins.");
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