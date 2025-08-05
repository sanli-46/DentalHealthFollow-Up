using DentalHealthFollow_Up.Shared.DTOs;
using DentalHealthFollow_Up.Entities;
using Microsoft.AspNetCore.Mvc;
using DentalHealthFollow_Up.DataAccess;

namespace DentalHealthFollow_Up.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalRecordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoalRecordsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoalRecord([FromBody] GoalRecordDto dto)
        {
            var record = new GoalRecord
            {
                GoalId = dto.GoalId,
                Note = dto.Note,
                CreatedAt = DateTime.Today.Add(dto.Time),
                DurationInMinutes = dto.DurationInMinutes,
                ImageBase64 = dto.ImageBase64,
                UserId = dto.UserId
            };

            _context.GoalRecords.Add(record);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
