using CryptoWalletApi.ViewModels;

namespace CryptoWalletApi.Services
{
    public class ViewModelManager
    {
        private InformationProcessService _informationProcessService;

        public ViewModelManager(InformationProcessService informationProcessService)
        {
            _informationProcessService = informationProcessService;
        }

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

            coinViewModels = await GetCurrentCoinPricesAsync(coinViewModels);
            coinViewModels = GetCoinPercentageChanges(coinViewModels);
            return coinViewModels;
        }

        // this one stays private because it depends on the viewModels having current prices set.
        private List<CoinViewModel> GetCoinPercentageChanges(List<CoinViewModel> coins)
        {
            var percentageChanges = _informationProcessService.CalculateValuePercentageChangeOfCoins(coins);

            foreach (var coin in coins)
                coin.PercentageChange = percentageChanges[coin.Id];

            return coins;
        }

        public async Task<List<CoinViewModel>> GetCurrentCoinPricesAsync(List<CoinViewModel> coins)
        {
            var currentPrices = await _informationProcessService.CalculateCurrentPriceOfCoinAsync(coins);

            foreach (var coin in coins)
                coin.CurrentPrice = currentPrices[coin.Id];

            return coins;
        }
    }
}