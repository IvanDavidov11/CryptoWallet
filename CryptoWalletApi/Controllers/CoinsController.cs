using CryptoWalletApi.Data;
using CryptoWalletApi.Services;
using CryptoWalletApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CryptoWalletApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private DatabaseManager _dbManager;
        public CoinsController(DatabaseContext context)
        {
            _dbManager = new DatabaseManager(context);
        }

        // GET: api/coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinViewModel>>> GetCoins()
        {
            if(_dbManager is null)
                return BadRequest(); // log error
            
            var coins = await _dbManager.GetOwnedCoinsAsync();
            
            if(coins is null)
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
                return BadRequest("No file uploaded.");
            }

            var result = await _dbManager.SeedDbWithCoins(file);
            return result ? Ok(result) : NoContent();
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearPortfolio()
        {
            var result = await _dbManager.ClearCoinsFromDb();
            return result ? Ok() : BadRequest();
        }
    }
}
