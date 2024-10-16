namespace CryptoWalletApi.DataTransferObjects
{
    public class ApiCoinWithNameAndSymbolDTO
    {
        public ApiCoinWithNameAndSymbolDTO(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }

        public string Name { get; set; }

        public string Symbol { get; set; }
    }
}
