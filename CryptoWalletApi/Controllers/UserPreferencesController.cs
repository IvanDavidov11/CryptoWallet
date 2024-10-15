using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers
{
    [Route("api/prefs")]
    [ApiController]
    public class UserPreferencesController : ControllerBase
    {
        private DatabaseManager _dbManager;

        public UserPreferencesController(DatabaseContext dbContext)
        {
            _dbManager = new DatabaseManager(dbContext);
        }

        [HttpGet]
        public async Task<ActionResult<UserPreferencesDatabaseModel>> GetPreferences()
        {
            if (_dbManager is null)
                return BadRequest(); // log error

            var userPreferences = await _dbManager.GetUserPreferencesAsync();

            return Ok(userPreferences);
        }
    }
}