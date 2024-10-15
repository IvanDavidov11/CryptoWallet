using CryptoWalletApi.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletApi.Data.DbModels
{
    public class UserPreferencesDatabaseModel : IUserPreferences
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int RefreshInterval { get; set; } = DataConstants.DefaultPortfolioRefreshInterval;
    }
}