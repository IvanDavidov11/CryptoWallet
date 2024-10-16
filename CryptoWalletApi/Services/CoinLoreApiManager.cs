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
        private ILogger _logger;

        public CoinLoreApiManager(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns a DTO with GlobalData from CoinLoreApi
        /// </summary>
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
        /// Returns 100 coins from CoinLoreApi database.
        /// </summary>
        /// <param name="urlIndex">starting point of coins for api call</param>
        public async Task<List<CoinLoreCoinDTO>> GetCoinsFromApiAsync(int urlIndex = 0)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{CoinLoreGetCoinsUri}?start={urlIndex}&limit=100")
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(body))
                    throw new Exception();

                var allCoins = JsonConvert.DeserializeObject<ApiResponseCoinsDTO>(body).AllCoins;
                return allCoins;
            }
        }

        /// <summary>
        /// Returns information DTO for a single coin.
        /// </summary>
        public async Task<CoinLoreCoinDTO> GetSingleCoinInformationFromApiAsync(string coinLoreId)
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

        /// <summary>
        /// Returns collection of information DTOs for many coins.
        /// </summary>
        public async Task<ICollection<CoinLoreCoinDTO>> GetCoinsInformationFromApiAsync(ICollection<string> coinLoreIds)
        {
            _logger.LogInformation("Getting Coins information from CoinLore Api...");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CoinLoreGetSpecificCoinUrl + string.Join(',', coinLoreIds))
            };

            _logger.LogInformation("Attempting to send request to CoinLoreApi...");
            using (var response = await _client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    _logger.LogInformation($"Request succeeded. Status code: {response.StatusCode}");
                    _logger.LogInformation("Attempting to read information...");
                    var body = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(body))
                    {
                        _logger.LogWarning("Response body is empty.");
                        return new List<CoinLoreCoinDTO>();
                    }

                    _logger.LogInformation($"Information read successfully: {body}");
                    _logger.LogInformation($"Attempting to deserialize json response into List<CoinLoreCoinDTO>...");
                    var allResponseCoinsDTO = JsonConvert.DeserializeObject<List<CoinLoreCoinDTO>>(body);

                    if (allResponseCoinsDTO is null)
                    {
                        _logger.LogWarning("Deserialization returned null. Response might not be in expected format.");
                        return new List<CoinLoreCoinDTO>();
                    }

                    _logger.LogInformation($"Response successfully deserialized into expected format.");
                    _logger.LogInformation($"Successfully retrieved {allResponseCoinsDTO.Count} coins.");
                    return allResponseCoinsDTO;
                }
                catch (Exception err)
                {
                    _logger.LogError($"An error occurred: {err.GetType().Name} - {err.Message}");
                    return new List<CoinLoreCoinDTO>();
                }
            }
        }
    }
}