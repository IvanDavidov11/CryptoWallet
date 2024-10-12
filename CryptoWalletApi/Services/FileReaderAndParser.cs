using CryptoWalletApi.Data.DbModels;

namespace CryptoWalletApi.Services
{
    public static class FileReaderAndParser
    {
        private static readonly char[] separators = new[] { '|', ','};

        public async static Task<List<string>> GetFileAsStringCollection (IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = await reader.ReadToEndAsync();

                return fileContent
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
        }

        public static async Task<List<CoinModel>> MapFileToCoinDbModels (IFormFile file)
        {
            var allCoinsAsStrings = await GetFileAsStringCollection(file);
            if (allCoinsAsStrings == null)
                return null;

            var allCoins = new List<CoinModel>();

            foreach (var coinInfo in allCoinsAsStrings)
            {
                string[] coin = coinInfo.Split(separators);

                if (coin.Length != 3)
                    continue; //add logic for bad coins

                var coinBoughtPrice = decimal.Parse(coin[0]);
                var coinName = coin[1];
                var coinAmount = decimal.Parse(coin[2]);
                
                allCoins.Add(new CoinModel()
                {
                    Name = coinName,
                    Amount = coinAmount,
                    BuyPrice = coinBoughtPrice,
                });
            }

            return allCoins;
        }
    }
}
