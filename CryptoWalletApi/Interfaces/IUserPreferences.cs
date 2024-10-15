namespace CryptoWalletApi.Interfaces
{
    public interface IUserPreferences
    {
        public int Id { get; set; }

        public int RefreshInterval { get; set; }
    }
}