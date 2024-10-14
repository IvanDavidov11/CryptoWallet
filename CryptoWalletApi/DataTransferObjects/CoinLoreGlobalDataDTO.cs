using Newtonsoft.Json;

namespace CryptoWalletApi.DataTransferObjects
{
    public class CoinLoreGlobalDataDTO
    {
        [JsonProperty("coins_count")]
        public int CoinsCount { get; set; }

        [JsonProperty("active_markets")]
        public int ActiveMarkets { get; set; }

        [JsonProperty("total_mcap")]
        public decimal TotalMcap { get; set; }

        [JsonProperty("total_volume")]
        public decimal TotalVolume { get; set; }

        [JsonProperty("btc_d")]
        public string BtcD { get; set; }

        [JsonProperty("eth_d")]
        public string EthD { get; set; }

        [JsonProperty("mcap_change")]
        public string McapChange { get; set; }

        [JsonProperty("volume_change")]
        public string VolumeChange { get; set; }

        [JsonProperty("avg_change_percent")]
        public string AvgChangePercent { get; set; }

        [JsonProperty("volume_ath")]
        public decimal VolumeAth { get; set; }

        [JsonProperty("mcap_ath")]
        public decimal McapAth { get; set; }
    }
}
