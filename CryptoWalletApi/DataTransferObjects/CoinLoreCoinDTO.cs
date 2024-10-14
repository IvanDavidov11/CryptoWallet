using Newtonsoft.Json;

namespace CryptoWalletApi.DataTransferObjects
{
    public class CoinLoreCoinDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameid")]
        public string NameId { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("price_usd")]
        public decimal PriceUsd { get; set; }

        [JsonProperty("percent_change_24h")]
        public decimal PercentChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public decimal PercentChange1h { get; set; }

        [JsonProperty("percent_change_7d")]
        public decimal PercentChange7d { get; set; }

        [JsonProperty("price_btc")]
        public decimal PriceBtc { get; set; }

        [JsonProperty("market_cap_usd")]
        public decimal MarketCapUsd { get; set; }

        [JsonProperty("volume24")]
        public decimal Volume24 { get; set; }

        [JsonProperty("volume24a")]
        public decimal Volume24a { get; set; }

        [JsonProperty("csupply")]
        public decimal? Csupply { get; set; }

        [JsonProperty("tsupply")]
        public decimal? Tsupply { get; set; }

        [JsonProperty("msupply")]
        public decimal? Msupply { get; set; }
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
        public List<CoinLoreCoinDTO> AllCoins { get; set; }

        [JsonProperty("info")]
        public InfoDTO Info { get; set; }
    }
}