using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;

namespace CryptoWalletApi.Services
{
    public static class FileReaderAndParser
    {
        private static readonly char[] separators = new[] { '|', ',' };
        private static int expectedCountOfParameters = 3;

        public async static Task<List<string>> GetFileAsStringCollectionAsync(ILogger logger, IFormFile file)
        {
            logger.LogInformation($"Attempting to read file: {file.FileName}");

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                try
                {
                    logger.LogInformation("Reading file content...");
                    var fileContent = await reader.ReadToEndAsync();

                    logger.LogInformation("File content read successfully. Splitting into lines...");
                    var lines = fileContent
                        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    logger.LogInformation($"Successfully split file content into {lines.Count} lines.");
                    return lines;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while reading the file: {file.FileName}");
                    return null;
                }
            }
        }

        public static async Task<CheckedCoinsDTO> MapFileToCoinDbModelsAsync(ILogger logger, IFormFile file)
        {
            logger.LogInformation("Mapping file data to CoinDatabaseModels started...");

            logger.LogInformation("Reading file content into a string collection...");
            List<string> allCoinsAsStrings = await GetFileAsStringCollectionAsync(logger, file);
            var allCoins = new CheckedCoinsDTO(null, null);

            if (allCoinsAsStrings == null)
            {
                logger.LogWarning($"Failed to read file content into a string collection. File: {file}");
                return allCoins;
            }

            logger.LogInformation($"Successfully retrieved file content. Starting to process {allCoinsAsStrings.Count} coins...");
            int successfulProcessing = 0;

            foreach (var coinInfo in allCoinsAsStrings)
            {
                logger.LogInformation($"Processing coin string: {coinInfo}");
                string[] coin = coinInfo.Split(separators);

                if (coin.Length != expectedCountOfParameters ||
                   !decimal.TryParse(coin[0], out decimal coinBoughtPrice) ||
                   !decimal.TryParse(coin[2], out decimal coinAmount))
                {
                    logger.LogWarning($"Coin string format invalid: {coinInfo}. Expected format: X.XXXX(number)|CoinName|X.XXXX(number)");
                    allCoins.BadCoins.Add(new CoinDatabaseModel() { Name = coinInfo, IsValid = false });
                    continue;
                }

                var coinName = coin[1];
                allCoins.GoodCoins.Add(new CoinDatabaseModel()
                {
                    Name = coinName,
                    Amount = coinAmount,
                    BuyPrice = coinBoughtPrice,
                });

                logger.LogInformation($"Successfully processed coin: {coinName}");
                successfulProcessing++;
            }

            logger.LogInformation($"Finished processing coins. Successfully processed {successfulProcessing}/{allCoinsAsStrings.Count} coins.");
            return allCoins;
        }
    }
}