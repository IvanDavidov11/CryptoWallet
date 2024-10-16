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
            _logger.LogInformation("Starting Api request to CoinLore for global data...");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CoinLoreGlobalDataUri)
            };

            try
            {
                _logger.LogInformation($"Sending request to URI: {request.RequestUri}");

                using (var response = await _client.SendAsync(request))
                {
                    _logger.LogInformation($"Received response from CoinLore. Status Code: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(body))
                    {
                        _logger.LogWarning("Response body is empty.");
                        throw new Exception("Empty response body.");
                    }

                    _logger.LogInformation("Deserializing response body...");
                    var globalData = JsonConvert.DeserializeObject<List<CoinLoreGlobalDataDTO>>(body).FirstOrDefault();

                    _logger.LogInformation("Successfully retrieved global data from CoinLore.");
                    return globalData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Returns 100 coins from CoinLoreApi database.
        /// </summary>
        /// <param name="urlIndex">starting point of coins of api call</param>
        public async Task<List<CoinLoreCoinDTO>> GetCoinsFromApiAsync(int urlIndex = 0)
        {
            _logger.LogInformation($"Starting request to CoinLore Api for 100 coins starting at index {urlIndex}."); // TODO: fix logging spam...

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{CoinLoreGetCoinsUri}?start={urlIndex}&limit=100")
            };

            try
            {
                _logger.LogInformation($"Sending request to CoinLore Api: {request.RequestUri}");

                using (var response = await _client.SendAsync(request))
                {
                    _logger.LogInformation($"Received response from CoinLore Api. Status Code: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(body))
                    {
                        _logger.LogWarning("Response body is empty.");
                        throw new Exception("Empty response body.");
                    }

                    _logger.LogInformation("Deserializing response body...");
                    var allCoins = JsonConvert.DeserializeObject<ApiResponseCoinsDTO>(body).AllCoins;

                    _logger.LogInformation($"Successfully retrieved {allCoins.Count} coins from CoinLore Api.");
                    return allCoins;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.GetType().Name} - {ex.Message}");
                throw;
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
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.GetType().Name} - {ex.Message}");
                    return new List<CoinLoreCoinDTO>();
                }
            }
        }
    }
}