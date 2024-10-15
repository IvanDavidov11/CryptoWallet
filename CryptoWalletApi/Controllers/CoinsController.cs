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

        public CoinsController(DatabaseContext context)
        {
            _dbManager = new DatabaseManager(context);
            _informationProcessService = new InformationProcessService();
            _viewModelManager = new ViewModelManager(_informationProcessService);
        }

        // GET: api/coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinViewModel>>> GetCoins()
        {
            var coins = await _viewModelManager.GenerateCoinViewModelsAsync(_dbManager);

            if (coins is null)
                return NoContent(); // log error

            return Ok(coins);
        }

        [HttpGet("has-coins")]
        public ActionResult<bool> HasCoins()
        {
            return Ok(_dbManager.DbHasCoins());
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadPortfolio(IFormFile file)
        {
            CheckedCoinsDTO checkedCoins = await _informationProcessService.CheckValidityOfCoinFile(file);

            if (checkedCoins.BadCoins.Count > 0)
            {
                return StatusCode(206, new
                {
                    goodCoins = checkedCoins.GoodCoins,
                    badCoins = checkedCoins.BadCoins,
                });
            }

            var successfulyAddedToDb = await _dbManager.SeedDbWithCoinsFileAsync(checkedCoins.GoodCoins);

            return successfulyAddedToDb ? Ok("Coins added successfully.") : StatusCode(500, "Error adding coins.");
        }

        [HttpPost("upload-safe")]
        public async Task<ActionResult> UploadPortfolioWithSafeCoins([FromBody] List<CoinDatabaseModel> goodCoins)
        {
            if (goodCoins == null || !goodCoins.Any())
                return BadRequest("No good coins provided.");

            var successfullyAdded = await _dbManager.SeedDbWithCoinsFileAsync(goodCoins);

            return successfullyAdded ? Ok("Good coins added successfully.") : StatusCode(500, "Error adding good coins.");
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearPortfolio()
        {
            var result = await _dbManager.ClearCoinsFromDbAsync();
            return result ? Ok() : BadRequest();
        }
    }
}