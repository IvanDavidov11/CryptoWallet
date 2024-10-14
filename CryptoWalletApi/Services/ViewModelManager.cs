using CryptoWalletApi.ViewModels;

namespace CryptoWalletApi.Services
{
    public class ViewModelManager
    {
        private InformationProcessService _informationProcessService = new();

        public async Task<IEnumerable<CoinViewModel>> GenerateCoinViewModelsAsync(DatabaseManager dbManager)
        {
            IEnumerable<Data.DbModels.CoinModel> coinDbModels = await dbManager.GetOwnedCoinsAsync();
            var coinViewModels = coinDbModels.Select(x => new CoinViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amount,
                BuyPrice = x.BuyPrice,
                CoinLoreId = x.CoinLoreId,
            }).ToList();

            coinViewModels = await UpdateCoinPercentageChangesAsync(dbManager, coinViewModels);
            return coinViewModels;
        }

        public async Task<List<CoinViewModel>> UpdateCoinPercentageChangesAsync(DatabaseManager dbManager, List<CoinViewModel> coins)
        {
            var percentageChanges = await _informationProcessService.CalculateValuePercentageChangeOfCoinsAsync(coins);

            foreach (var coin in coins)
            {
                coin.PercentageChange = percentageChanges[coin.Id];
            }

            return coins;
        }
    }
}