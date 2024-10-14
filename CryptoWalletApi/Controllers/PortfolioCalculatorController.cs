using CryptoWalletApi.Data;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers
{
    [Route("api/calc")]
    [ApiController]
    public class PortfolioCalculatorController : ControllerBase
    {
        private DatabaseManager _dbManager;
        private InformationProcessService _infoProcessService = new();
        public PortfolioCalculatorController(DatabaseContext context)
        {
            _dbManager = new DatabaseManager(context);
        }

        [HttpGet("initial")]
        public async Task<ActionResult> CalculateInitialPortfolioValue()
        {
            var coins = await _dbManager.GetOwnedCoinsAsync();
            decimal initialValue = await _infoProcessService.CalculateInitialPortfolioValueAsync(coins);

            return Ok(initialValue);
        }

        [HttpGet("current")]
        public async Task<ActionResult> CalculateCurrentPortfolioValue()
        {
            var coins = await _dbManager.GetOwnedCoinsAsync();
            decimal currentValue = await _infoProcessService.CalculateCurrentPortfolioValueAsync(coins.ToList());

            return Ok(currentValue);
        }
    }
}