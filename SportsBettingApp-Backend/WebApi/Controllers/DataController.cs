using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Application.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {


        private readonly ILogger<DataController> _logger;
        private readonly DataService _dataService;

        public DataController(ILogger<DataController> logger, DataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }


        [Route("sports")]
        [HttpGet]
        public async Task<IEnumerable<Sport>> GetSportsAsync()
        {
           return await _dataService.GetSportsAsync();
        }

        [Route("betting-days")]
        [HttpGet]
        public async Task<IEnumerable<BettingDay>> GetBettingDaysAsync()
        {
            return await _dataService.GetBettingDaysAsync();
        }

        [Route("betting-pairs")]
        [HttpGet]
        public async Task<IEnumerable<BettingPair>> GetBettingPairsAsync()
        {            
            return await _dataService.GetBettingPairsAsync();
        }

        [Route("special-offer")]
        [HttpGet]
        public async Task<IEnumerable<SpecialOffer>> GetSpecialOfferAsync()
        {
            return await _dataService.GetSpecialOfferAsync();
        }
    }
}