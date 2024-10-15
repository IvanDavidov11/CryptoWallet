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
        private InformationProcessService _informationProcessService;

        public PortfolioCalculatorController(DatabaseContext context)
        {
            _dbManager = new DatabaseManager(context);
            _informationProcessService = new InformationProcessService();
        }

        [HttpGet("initial")]
        public async Task<ActionResult> CalculateInitialPortfolioValue()
        {
            var coins = await _dbManager.GetOwnedCoinsAsync();
            decimal initialValue = _informationProcessService.CalculateInitialPortfolioValue(coins);

            return Ok(initialValue);
        }

        [HttpGet("current")]
        public async Task<ActionResult> CalculateCurrentPortfolioValue()
        {
            var coins = await _dbManager.GetOwnedCoinsAsync();
            decimal currentValue = await _informationProcessService.CalculateCurrentPortfolioValueAsync(coins.ToList());

            return Ok(currentValue);
        }
    }
}