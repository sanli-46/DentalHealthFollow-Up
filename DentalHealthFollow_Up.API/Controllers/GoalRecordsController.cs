using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using DentalHealthFollow_Up.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthFollow_Up.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalRecordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoalRecordsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GoalRecordCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var goalExists = await _context.Goals
                .AnyAsync(g => g.GoalId == dto.GoalId && g.UserId == dto.UserId);
            if (!goalExists) return BadRequest("Geçersiz hedef veya kullanıcı.");

            if (dto.Date == default)
                dto.Date = DateTime.Today;

            var record = new GoalRecord
            {
                GoalId = dto.GoalId,
                UserId = dto.UserId,
                Date = dto.Date.Date,
                DurationInMinutes = dto.DurationInMinutes <= 0 ? null : dto.DurationInMinutes,
                Note = dto.Note?.Trim(),
                ImageBase64 = string.IsNullOrWhiteSpace(dto.ImageBase64) ? null : dto.ImageBase64,
                CreatedAt = DateTime.UtcNow
            };

            _context.GoalRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok(new { record.GoalRecordId });
        }

        [HttpGet("last7days/{userId:int}")]
        public async Task<IActionResult> Last7Days(int userId)
        {
            var start = DateTime.Today.AddDays(-6);
            var list = await _context.GoalRecords
                .AsNoTracking()
                .Where(r => r.UserId == userId && r.Date >= start)
                .OrderByDescending(r => r.Date)
                .Select(r => new GoalRecordDto
                {
                    Id = r.GoalRecordId,
                    GoalId = r.GoalId,
                    UserId = r.UserId,
                    Date = r.Date,
                    DurationInMinutes = r.DurationInMinutes,
                    Note = r.Note,
                    ImageBase64 = r.ImageBase64,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }
    }
}

