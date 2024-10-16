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
        private ILogger _logger;

        public PortfolioCalculatorController(ILogger<CoinsController> logger, DatabaseContext context)
        {
            _logger = logger;
            _dbManager = new DatabaseManager(context, logger);
            _informationProcessService = new InformationProcessService(logger);
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