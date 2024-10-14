using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.ViewModels;

namespace CryptoWalletApi.Services
{
    public class ViewModelManager
    {
        private InformationProcessService _informationProcessService = new();

        public async Task<IEnumerable<CoinViewModel>> GenerateCoinViewModelsAsync(DatabaseManager dbManager)
        {
            IEnumerable<Data.DbModels.CoinDatabaseModel> coinDbModels = await dbManager.GetOwnedCoinsAsync();
            var coinViewModels = coinDbModels.Select(x => new CoinViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amount,
                BuyPrice = x.BuyPrice,
                CoinLoreId = x.CoinLoreId,
            }).ToList();

            coinViewModels = await GetCurrentCoinPricesAsync(dbManager, coinViewModels);
            coinViewModels = await GetCoinPercentageChangesAsync(dbManager, coinViewModels);
            return coinViewModels;
        }

        // this one stays private because it depends on the viewModels having current prices set.
        private async Task<List<CoinViewModel>> GetCoinPercentageChangesAsync(DatabaseManager dbManager, List<CoinViewModel> coins)
        {
            var percentageChanges = await _informationProcessService.CalculateValuePercentageChangeOfCoinsAsync(coins);

            foreach (var coin in coins)
            {
                coin.PercentageChange = percentageChanges[coin.Id];
            }

            return coins;
        }

        public async Task<List<CoinViewModel>> GetCurrentCoinPricesAsync(DatabaseManager dbManager, List<CoinViewModel> coins)
        {
            var currentPrices = await _informationProcessService.CalculateCoinCurrentPriceAsync(coins);

            foreach (var coin in coins)
            {
                coin.CurrentPrice = currentPrices[coin.Id];
            }

            return coins;
        }

        //public async Task<List<CoinViewModel>> GetCurrentCoinPricesAsync(DatabaseManager dbManager)
        //{
        //    var coins = await GenerateCoinViewModelsAsync(dbManager);
        //    var currentPrices = await _informationProcessService.CalculateCoinCurrentPriceAsync(coins);

        //    foreach (var coin in coins)
        //    {
        //        coin.CurrentPrice = currentPrices[coin.Id];
        //    }

        //    return coins.ToList();
        //}
    }
}