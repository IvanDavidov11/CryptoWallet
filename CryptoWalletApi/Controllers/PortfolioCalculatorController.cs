using CryptoWalletApi.Data;
using CryptoWalletApi.DataTransferObjects;
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
            _logger.LogInformation($"Entering CalculateInitialPortfolioValue method");

            var coins = await _dbManager.GetOwnedCoinsAsync();
            decimal initialValue = _informationProcessService.CalculateInitialPortfolioValue(coins);

            _logger.LogInformation("Sending initial value to front-end.");
            return Ok(initialValue);
        }

        [HttpGet("current")]
        public async Task<ActionResult> CalculateCurrentPortfolioValue()
        {
            _logger.LogInformation($"Entering CalculateCurrentPortfolioValue method");

            var coins = await _dbManager.GetOwnedCoinsAsync();
            
            decimal currentValue = await _informationProcessService.CalculateCurrentPortfolioValueAsync(coins);
            string percentageChange = _informationProcessService.CalculatePercentageChangeOfPortfolio(currentValue, coins);

            string combinedValue = $"{currentValue:f2}   {percentageChange}";

            _logger.LogInformation("Sending current portfolio value to front-end.");
            return Ok(combinedValue);
        }
    }
}