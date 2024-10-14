using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using Newtonsoft.Json;

namespace CryptoWalletApi.Services
{
    public class CoinLoreApiManager
    {
        private static HttpClient _client = new HttpClient();
        private const string CoinLoreGlobalDataUri = @"https://api.coinlore.net/api/global/";
        private const string CoinLoreGetCoinsUri = @" https://api.coinlore.net/api/tickers/";

        public async Task<CoinLoreGlobalDataDTO> GetGlobalDataFromApiAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CoinLoreGlobalDataUri)
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(body))
                    throw new Exception();

                var globalData = JsonConvert.DeserializeObject<List<CoinLoreGlobalDataDTO>>(body).FirstOrDefault();
                return globalData;
            }
        }

        public async Task<Dictionary<CoinModel, bool>> TryFindCoinsInApiAsync(ICollection<CoinModel> coinNames)
        {
            // will use later for deeper searching
            // var amountOfCoins = GetGlobalDataFromApiAsync().Result.CoinsCount;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CoinLoreGetCoinsUri)
            };

            using (var response = await _client.SendAsync(request))
            {
                List<CoinLoreCoinDTO> allApiCoins = await GetAllCoinsFromApi(response);

                var apiCoinDictionaryWithName = allApiCoins.ToDictionary(c => c.Name, c => c.Id, StringComparer.OrdinalIgnoreCase);
                var apiCoinDictionaryWithSymbol = allApiCoins.ToDictionary(c => c.Symbol, c => c.Id, StringComparer.OrdinalIgnoreCase);

                Dictionary<CoinModel, bool> checkedCoins = new();

                foreach (var coin in coinNames)
                {
                    var coinName = coin.Name;

                    if (apiCoinDictionaryWithName.TryGetValue(coinName, out var coinId) ||
                        apiCoinDictionaryWithSymbol.TryGetValue(coinName, out coinId))
                    {
                        coin.CoinLoreId = coinId;
                        checkedCoins.Add(coin, true);
                    }
                    else
                    {
                        checkedCoins.Add(coin, false);
                    }
                }

                return checkedCoins;
            }
        }

        private async Task<List<CoinLoreCoinDTO>> GetAllCoinsFromApi(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(body))
                throw new Exception();

            var allCoins = JsonConvert.DeserializeObject<ApiResponseCoinsDTO>(body).AllCoins;
            return allCoins;
        }
    }
}
