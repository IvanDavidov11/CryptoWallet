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
        private InformationProcessService _informationProcessService = new();
        private ViewModelManager _viewModelManager = new();

        public CoinsController(DatabaseContext context)
        {
            _dbManager = new DatabaseManager(context);
        }

        // GET: api/coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinViewModel>>> GetCoins()
        {
            if (_dbManager is null)
                return BadRequest(); // log error

            var coins = await _viewModelManager.GenerateCoinViewModelsAsync(_dbManager);

            if (coins is null)
                return NoContent(); // log error

            return Ok(coins);
        }

        [HttpGet("has-coins")]
        public ActionResult<bool> HasCoins()
        {
            if (_dbManager is null)
                return BadRequest(); // log error

            return Ok(_dbManager.DbHasCoins());
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadPortfolio(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return NoContent();
            }

            CheckedCoinsDTO checkedCoins = await _informationProcessService.ProcessCoinFileAsync(file);

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
        public async Task<ActionResult> UploadPortfolioWithSafeCoins([FromBody] List<CoinModel> goodCoins)
        {
            if (_dbManager is null)
                return BadRequest(); // log error

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