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
            _logger.LogInformation("Attempting to check validity of coin files against CoinLoreApi...");

            CheckedCoinsDTO allCoins = await FileReaderAndParser.MapFileToCoinDbModelsAsync(_logger, file);
            _logger.LogInformation("Mapped file data to coin database models. " +
                $"GoodCoins count: {allCoins.GoodCoins.Count}, BadCoins count: {allCoins.BadCoins.Count}");

            var checkedCoins = new CheckedCoinsDTO(allCoins.GoodCoins, allCoins.BadCoins);

            if (allCoins.GoodCoins.Count == 0 && allCoins.BadCoins.Count == 0)
            {
                _logger.LogWarning("CoinDatabaseModels are empty. Please check if MapFielToCoinsDbModel executed correctly.");
                return checkedCoins;
            }

            _logger.LogInformation($"Verifying {allCoins.GoodCoins.Count} good coins against CoinLore API...");
            Dictionary<CoinDatabaseModel, bool> result = await VerifyCoinsWithCoinLoreApiAsync(allCoins.GoodCoins);

            foreach (var coin in result)
            {
                if (coin.Value == true)
                {
                    checkedCoins.GoodCoins.Add(coin.Key);
                    _logger.LogInformation($"Coin {coin.Key.Name} is valid and added to GoodCoins.");
                }
                else
                {
                    checkedCoins.BadCoins.Add(coin.Key);
                    _logger.LogInformation($"Coin {coin.Key.Name} is invalid and added to BadCoins.");
                }
            }

            _logger.LogInformation($"Finished checking validity of coin files. " +
                $"Total GoodCoins: {checkedCoins.GoodCoins.Count}, Total BadCoins: {checkedCoins.BadCoins.Count}");

            return checkedCoins;
        }

        /// <summary>
        /// Loops through all the coins in CoinLoreApi, until it verifies all coinsToCheck, or it reaches the end.
        /// </summary>
        public async Task<Dictionary<CoinDatabaseModel, bool>> VerifyCoinsWithCoinLoreApiAsync(ICollection<CoinDatabaseModel> coinsToCheck)
        {
            _logger.LogInformation("Attempting to verify coins with CoinLore Api...");

            int amountOfCoinsInCoinLore = (await _coinLoreApiManager.GetGlobalDataFromApiAsync()).CoinsCount;
            _logger.LogInformation($"Total coins in CoinLore Api: {amountOfCoinsInCoinLore}");

            Dictionary<CoinDatabaseModel, bool> overallCheckedCoins = new();

            for (int index = 0; index < amountOfCoinsInCoinLore; index += 100)
            {
                _logger.LogInformation($"Fetching coins from CoinLore Api starting at index: {index}");

                List<CoinLoreCoinDTO> responseApiCoins = await _coinLoreApiManager.GetCoinsFromApiAsync(index);

                _logger.LogInformation("Verifying coins against Api response...");
                Dictionary<CoinDatabaseModel, bool> checkedCoins = VerifyCoinsAgainstApiResponse(coinsToCheck, responseApiCoins);
                IEnumerable<KeyValuePair<CoinDatabaseModel, bool>> verifiedCoins = checkedCoins.Where(dto => dto.Value == true);

                foreach (var verifiedCoin in verifiedCoins)
                {
                    overallCheckedCoins.Add(verifiedCoin.Key, verifiedCoin.Value);
                    coinsToCheck.Remove(verifiedCoin.Key);
                    _logger.LogInformation($"Coin: {verifiedCoin.Key.Name} has been verified and is removed from coinsToCheck collection.");
                }

                if (!coinsToCheck.Any())
                {
                    _logger.LogInformation("All coins have been verified. Exiting loopp early.");
                    break;
                }
            }

            if (coinsToCheck.Any())
            {
                _logger.LogWarning($"Some coins could not be verified: {coinsToCheck.Count}.");
                
                foreach (var badCoin in coinsToCheck)
                {
                    overallCheckedCoins.Add(badCoin, false);
                    _logger.LogInformation($"Adding unverified coin: {badCoin.Name} to BadCoin collection.");
                }
            }

            _logger.LogInformation($"Attempt to verify coins has completed. " +
                $"Coins successfully verified: {overallCheckedCoins.Count(coin => coin.Value == true)}.");

            return overallCheckedCoins;
        }

        /// <summary>
        /// Tries to match the name or symbol of coinsToCheck against apiResponseCoins.
        /// </summary>
        public Dictionary<CoinDatabaseModel, bool> VerifyCoinsAgainstApiResponse(
            ICollection<CoinDatabaseModel> coinsToCheck,
            ICollection<CoinLoreCoinDTO> apiResponseCoins)
        {
            _logger.LogInformation("Starting verification of coins against Api response...");

            Dictionary<string, ApiCoinWithNameAndSymbolDTO> apiResponseCoinWithNameAndSymbol = apiResponseCoins.ToDictionary(
                c => c.Id,
                c => new ApiCoinWithNameAndSymbolDTO(c.Name, c.Symbol),
                StringComparer.OrdinalIgnoreCase);

            if (coinsToCheck.Count != apiResponseCoins.Count)
                _logger.LogWarning("Amount of coins to check doesn't match amount of coins in response api.");
            
            _logger.LogInformation($"Total coins to check: {coinsToCheck.Count} / Total coins in Api response: {apiResponseCoins.Count}");

            Dictionary<CoinDatabaseModel, bool> checkedCoins = new();
            
            foreach (var coin in coinsToCheck)
            {
                var coinName = coin.Name;
                _logger.LogInformation($"Verifying coin: {coinName}");

                bool coinFound = false;

                foreach (var apiCoin in apiResponseCoinWithNameAndSymbol)
                {
                    if (apiCoin.Value.Name == coinName || apiCoin.Value.Symbol == coinName)
                    {
                        coin.CoinLoreId = apiCoin.Key;
                        checkedCoins.Add(coin, true);
                        coinFound = true;
                        _logger.LogInformation($"Match found for coin: {coinName}. CoinLoreId set to: {apiCoin.Key}");
                        break;
                    }
                }

                if (!coinFound)
                {
                    checkedCoins.Add(coin, false);
                    _logger.LogWarning($"No match found for coin: {coinName}.");
                }
            }

            _logger.LogInformation($"Verification complete. Total checked coins: {checkedCoins.Count}.");
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

                _logger.LogInformation($"Coin: {coin.Name} percentage change calculated as {percentage}");
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
                _logger.LogWarning("No coin information was retrieved from the Api.");
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