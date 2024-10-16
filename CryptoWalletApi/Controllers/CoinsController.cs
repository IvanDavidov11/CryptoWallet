using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
using CryptoWalletApi.DataTransferObjects;
using CryptoWalletApi.Services;
using CryptoWalletApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private DatabaseManager _dbManager;
        private InformationProcessService _informationProcessService;
        private ViewModelManager _viewModelManager;
        private ILogger _logger;

        public CoinsController(ILogger<CoinsController> logger, DatabaseContext context)
        {
            _logger = logger;
            _dbManager = new DatabaseManager(context, logger);
            _informationProcessService = new InformationProcessService(logger);
            _viewModelManager = new ViewModelManager(logger, _informationProcessService);
        }

        // GET: api/coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinViewModel>>> GetCoins()
        {
            _logger.LogInformation("Entering GetCoins method...");

            IEnumerable<CoinViewModel>? coins = await _viewModelManager.MapDatabaseModelsToViewModelsAsync(_dbManager);

            if (coins is null)
            {
                _logger.LogError("CoinViewModels couldn't be generated.");
                return NoContent();
            }

            _logger.LogInformation("Getting all coins finished successfully, sending all coins to front-end.");
            return Ok(coins);
        }

        [HttpGet("has-coins")]
        public ActionResult<bool> HasCoins()
        {
            _logger.LogInformation("Entering HasCoins method...");
            return Ok(_dbManager.DbHasCoins());
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadPortfolio(IFormFile file)
        {
            _logger.LogInformation($"Entering UploadPortfolio method with file: {file}");

            CheckedCoinsDTO checkedCoins = await _informationProcessService.CheckValidityOfCoinFile(file);

            if (checkedCoins.BadCoins.Count > 0)
            {
                _logger.LogWarning("Bad coins detected. Sending partial content code (206) with both good and bad coins to front-end.");

                return StatusCode(206, new
                {
                    goodCoins = checkedCoins.GoodCoins,
                    badCoins = checkedCoins.BadCoins,
                });
            }

            _logger.LogInformation("All coins have been validated with CoinLore Api. Attempting to save them to database...");
            bool successfulyAddedToDb = await _dbManager.SeedDbWithCoinsFileAsync(checkedCoins.GoodCoins);

            return successfulyAddedToDb ? Ok("Coins added successfully.") : StatusCode(500, "Error adding coins.");
        }

        [HttpPost("upload-safe")]
        public async Task<ActionResult> UploadPortfolioWithSafeCoins([FromBody] List<CoinDatabaseModel> goodCoins)
        {
            if (goodCoins == null || !goodCoins.Any())
                return BadRequest("No good coins provided.");

            bool successfullyAdded = await _dbManager.SeedDbWithCoinsFileAsync(goodCoins);

            return successfullyAdded ? Ok("Good coins added successfully.") : StatusCode(500, "Error adding good coins.");
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearPortfolio()
        {
            bool result = await _dbManager.ClearCoinsFromDbAsync();
            return result ? Ok() : BadRequest();
        }
    }
}