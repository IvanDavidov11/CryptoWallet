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
        private const string CoinLoreGetSpecificCoinUrl = @"https://api.coinlore.net/api/ticker/?id=";

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

        /// <summary>
        /// Gets 100 coins from CoinLore and verifies coinsToCheck against them.
        /// </summary>
        public async Task<Dictionary<CoinDatabaseModel, bool>> VerifyCoinsAgainstApiAsync(ICollection<CoinDatabaseModel> coinsToCheck, int urlIndex = 0)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{CoinLoreGetCoinsUri}?start={urlIndex}&limit=100")
            };

            using (var response = await _client.SendAsync(request))
            {
                List<CoinLoreCoinDTO> allApiCoins = await GetAllCoinsFromApiAsync(response);

                var apiCoinDictionaryWithName = allApiCoins.ToDictionary(c => c.Id, c => c.Name, StringComparer.OrdinalIgnoreCase);
                var apiCoinDictionaryWithSymbol = allApiCoins.ToDictionary(c => c.Id, c => c.Symbol,StringComparer.OrdinalIgnoreCase);

                Dictionary<CoinDatabaseModel, bool> checkedCoins = new();

                foreach (var coin in coinsToCheck)
                {
                    var coinName = coin.Name;
                    bool coinFound = false;

                    foreach (var apiCoin in apiCoinDictionaryWithName)
                    {
                        if (apiCoin.Value == coinName || apiCoinDictionaryWithSymbol[apiCoin.Key] == coinName)
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
        }

        /// <summary>
        /// Loops through all the coins in CoinLore until it verifies all coinsToCheck, or it reaches the end.
        /// </summary>
        public async Task<Dictionary<CoinDatabaseModel, bool>> VerifyAgainstAllCoinsInApi(ICollection<CoinDatabaseModel> coinsToCheck)
        {
            int amountOfCoinsInCoinLore = (await GetGlobalDataFromApiAsync()).CoinsCount;
            Dictionary<CoinDatabaseModel, bool> overallCheckedCoins = new();

            for (int index = 0; index < amountOfCoinsInCoinLore; index += 100)
            {
                var checkedCoins = await VerifyCoinsAgainstApiAsync(coinsToCheck, index);
                var verifiedCoins = checkedCoins.Where(dto => dto.Value == true);

                foreach (var foundCoin in verifiedCoins)
                {
                    overallCheckedCoins.Add(foundCoin.Key, foundCoin.Value);
                    coinsToCheck.Remove(foundCoin.Key);
                }

                if (!coinsToCheck.Any())
                    break;
            }

            if (coinsToCheck.Any())
            {
                foreach (var badCoin in coinsToCheck)
                {
                    overallCheckedCoins.Add(badCoin, false);
                }
            }

            return overallCheckedCoins;
        }

        private async Task<List<CoinLoreCoinDTO>> GetAllCoinsFromApiAsync(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(body))
                throw new Exception();

            var allCoins = JsonConvert.DeserializeObject<ApiResponseCoinsDTO>(body).AllCoins;
            return allCoins;
        }

        public async Task<CoinLoreCoinDTO> GetCoinInformationFromApiAsync(string coinLoreId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CoinLoreGetSpecificCoinUrl + coinLoreId)
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(body))
                    throw new Exception();

                var foundCoin = JsonConvert.DeserializeObject<CoinLoreCoinDTO>(body);

                if (foundCoin is null)
                    throw new Exception();

                return foundCoin;
            }
        }

        public async Task<ICollection<CoinLoreCoinDTO>> GetCoinsInformationFromApiAsync(ICollection<string> coinLoreIds)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CoinLoreGetSpecificCoinUrl + string.Join(',', coinLoreIds))
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(body))
                    throw new Exception();

                var allResponseCoinsDTO = JsonConvert.DeserializeObject<List<CoinLoreCoinDTO>>(body);

                if (allResponseCoinsDTO is null)
                    throw new Exception();

                return allResponseCoinsDTO;
            }
        }
    }
}