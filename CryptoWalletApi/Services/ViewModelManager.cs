using CryptoWalletApi.Controllers;
using CryptoWalletApi.ViewModels;
using Microsoft.Extensions.Logging;

namespace CryptoWalletApi.Services
{
    public class ViewModelManager
    {
        private InformationProcessService _informationProcessService;
        private ILogger _logger;

        public ViewModelManager(ILogger<CoinsController> logger, InformationProcessService informationProcessService)
        {
            _logger = logger;
            _informationProcessService = informationProcessService;
        }

        public async Task<IEnumerable<CoinViewModel>> MapDatabaseModelsToViewModelsAsync(DatabaseManager dbManager)
        {
            _logger.LogInformation("Attempting to GenerateCoinViewModels, this will map CoinDatabaseModels into CoinViewModels...");

            IEnumerable<Data.DbModels.CoinDatabaseModel> coinDbModels = await dbManager.GetOwnedCoinsAsync();
            var coinViewModels = coinDbModels.Select(x => new CoinViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amount,
                BuyPrice = x.BuyPrice,
                CoinLoreId = x.CoinLoreId,
            }).ToList();

            coinViewModels = await GetCurrentCoinPricesAsync(coinViewModels);
            coinViewModels = GetCoinPercentageChanges(coinViewModels);

            _logger.LogInformation("Generation of CoinViewModels has finished.");
            return coinViewModels;
        }

        // Private because it depends on the viewModels having current prices set.
        private List<CoinViewModel> GetCoinPercentageChanges(List<CoinViewModel> coins)
        {
            _logger.LogInformation("Attempting to get coin price percentage change from initial buy...");
            var percentageChanges = _informationProcessService.CalculateValuePercentageChangeOfCoins(coins);

            foreach (var coin in coins)
            {
                if (percentageChanges.TryGetValue(coin.Id, out string? percentageChange))
                {
                    coin.PercentageChange = percentageChange;
                    _logger.LogInformation($"Coin: {coin.Name} (ID: {coin.Id}) - Percentage Change set to: {percentageChange}");
                }
                else
                {
                    _logger.LogWarning($"Percentage change for Coin: {coin.Name} (ID: {coin.Id}) not found.");
                }
            }

            _logger.LogInformation("Finished updating percentage changes for coins.");
            return coins;
        }

        public async Task<List<CoinViewModel>> GetCurrentCoinPricesAsync(List<CoinViewModel> coins)
        {
            _logger.LogInformation("Attempting to get current coin prices...");
            var currentPrices = await _informationProcessService.CalculateCurrentPriceOfCoinAsync(coins);

            if (currentPrices.Count == 0)
            {
                _logger.LogWarning("Getting current coin prices has failed.");
                return coins;
            }
            
            foreach (var coin in coins)
            {
                if (!currentPrices.TryGetValue(coin.Id, out var price))
                {
                    _logger.LogWarning($"Coin: {coin.Name} ID {coin.Id} not found in current prices.");
                    continue;
                }

                coin.CurrentPrice = price;
                _logger.LogInformation($"Coin: {coin.Name} price set to: {coin.CurrentPrice}");
            }

            _logger.LogInformation("Getting current coin prices has finished.");
            return coins;
        }
    }
}