using CryptoWalletApi.Data;
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
        public CoinsController(DatabaseContext context)
        {
            _dbManager = new DatabaseManager(context);
        }

        // GET: api/coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinViewModel>>> GetCoins()
        {
            if(_dbManager is null)
                return BadRequest(); //log error
            
            var coins = await _dbManager.GetOwnedCoinsAsync();
            
            if(coins is null)
                return NoContent(); //log error

            return Ok(coins);
        }

        //// POST: api/coins
        //[HttpPost]
        //public async Task<ActionResult<CoinModel>> PostCoin(CoinModel coin)
        //{
        //    _DbContext.Coins.Add(coin);
        //    await _DbContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetCoins), new { id = coin.Id }, coin);
        //}
    }
}
