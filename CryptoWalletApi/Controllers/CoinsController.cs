using CryptoWalletApi.Data;
using CryptoWalletApi.Data.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly DatabaseContext _DbContext;
        public CoinsController(DatabaseContext context)
        {
            _DbContext = context;
        }

        // GET: api/coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinModel>>> GetCoins()
        {
            return await _DbContext.Coins.ToListAsync();
        }

        // POST: api/coins
        [HttpPost]
        public async Task<ActionResult<CoinModel>> PostCoin(CoinModel coin)
        {
            _DbContext.Coins.Add(coin);
            await _DbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoins), new { id = coin.Id }, coin);
        }
    }
}
