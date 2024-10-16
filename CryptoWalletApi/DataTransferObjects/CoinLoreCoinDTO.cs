using Newtonsoft.Json;

namespace CryptoWalletApi.DataTransferObjects
{
    public class CoinLoreCoinDTO
    {
        private decimal priceUsd;

        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("symbol")]
        public required string Symbol { get; set; }

        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("nameid")]
        public string? NameId { get; set; }

        [JsonProperty("rank")]
        public int? Rank { get; set; }

        [JsonProperty("price_usd")]
        public string PriceUsdString
        {
            get => priceUsd.ToString();
            set
            {
                if (decimal.TryParse(value, out var result))
                {
                    priceUsd = result;
                }
                else
                {
                    priceUsd = 0;
                }
            }
        }

        public decimal PriceUsd => priceUsd;

        [JsonProperty("percent_change_24h")]
        public string? PercentChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public string? PercentChange1h { get; set; }

        [JsonProperty("percent_change_7d")]
        public string? PercentChange7d { get; set; }

        [JsonProperty("price_btc")]
        public string? PriceBtc { get; set; }

        [JsonProperty("market_cap_usd")]
        public string? MarketCapUsd { get; set; }

        [JsonProperty("volume24")]
        public string? Volume24 { get; set; }

        [JsonProperty("volume24a")]
        public string? Volume24a { get; set; }

        [JsonProperty("csupply")]
        public string? Csupply { get; set; }

        [JsonProperty("tsupply")]
        public string? Tsupply { get; set; }

        [JsonProperty("msupply")]
        public string? Msupply { get; set; }

        public override string ToString()
        {
            return $"{Name}-CoinLoreId:({Id})";
        }
    }

    public class InfoDTO
    {
        [JsonProperty("coins_num")]
        public int CoinsNum { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public class ApiResponseCoinsDTO
    {
        [JsonProperty("data")]
        public required List<CoinLoreCoinDTO> AllCoins { get; set; }

        [JsonProperty("info")]
        public InfoDTO? Info { get; set; }
    }

}