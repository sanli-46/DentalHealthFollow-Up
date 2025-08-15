using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthFollow_Up.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthTipsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new();

        public HealthTipsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("random")]
        public async Task<ActionResult<HealthTip>> GetRandomTip()
        {
            var count = await _context.HealthTips.CountAsync();
            if (count == 0)
                return NotFound("Öneri bulunamadı.");

            var index = _random.Next(count);
            var randomTip = await _context.HealthTips.Skip(index).FirstOrDefaultAsync();

            return Ok(randomTip);
        }
    }
}

