using Microsoft.AspNetCore.Mvc;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplyFly_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SeatDetailController : ControllerBase
    {
        private readonly ISeatDetailService _seatDetailService;
        private readonly ILogger<SeatDetailController> _logger;

        public SeatDetailController(ISeatDetailService seatDetailService, ILogger<SeatDetailController> logger)
        {
            _seatDetailService = seatDetailService;
            _logger = logger;
        }

        [HttpGet]
        public Task<List<SeatDetail>> GetAllSeatDetails()
        {
            var seatDetails = _seatDetailService.GetAllSeatDetails();
            return seatDetails;
        }

        [HttpGet("ById")]
        public Task<SeatDetail> GetSeatDetailById(string seatNo)
        {
            var seatDetails = _seatDetailService.GetSeatDetailById(seatNo);
            return seatDetails;
        }

        [HttpPost]
        public async Task<SeatDetail> AddSeatDetail(SeatDetail seatDetail)
        {
            seatDetail = await _seatDetailService.AddSeatDetail(seatDetail);
            return seatDetail;
        }

        [HttpDelete]
        public async void DeleteSeatDetail(string seatNo)
        {
            bool isDeleted = await _seatDetailService.RemoveSeatDetail(seatNo);
            if (isDeleted)
            {
                Ok(new { message = "Seat deleted successfully" });
            }
            else
            {
                NotFound(new { message = "Seat not found" });
            }
        }
    }
}
