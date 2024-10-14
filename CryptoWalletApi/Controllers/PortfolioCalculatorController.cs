using CryptoWalletApi.Data;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Http;
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
            decimal initialValue = await _infoProcessService.CalculateInitialPortfolioValueAsync(_dbManager);

            return Ok(initialValue);
        }
    }
}
