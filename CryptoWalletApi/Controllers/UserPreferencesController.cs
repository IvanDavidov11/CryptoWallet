using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers
{
    [Route("api/prefs")]
    [ApiController]
    public class UserPreferencesController : ControllerBase
    {
        private DatabaseManager _dbManager;
        private ILogger _logger;

        public UserPreferencesController(ILogger<CoinsController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbManager = new DatabaseManager(dbContext, logger);
        }

        [HttpGet]
        public async Task<ActionResult<UserPreferencesDatabaseModel>> GetPreferences()
        {
            _logger.LogInformation("Entering GetPreferences method...");
            var userPreferences = await _dbManager.GetUserPreferencesAsync();

            _logger.LogInformation("Returning UserPreferences to front-end.");
            return Ok(userPreferences);
        }

        [HttpPatch("set-interval")]
        public async Task<ActionResult> UpdateRefreshInterval([FromBody] UpdateUserPreferencesDTO preferencesDTO)
        {
            _logger.LogInformation("Entering UpdateRefreshInterval method...");

            var success = await _dbManager.UpdateRefreshIntervalOfUser(preferencesDTO.RefreshInterval);
            return success? Ok() : BadRequest(); // log error
        }
    }
}