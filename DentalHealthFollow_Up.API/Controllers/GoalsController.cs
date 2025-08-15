using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using DentalHealthFollow_Up.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DentalHealthFollow_Up.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoalsController(AppDbContext context)
        {
            _context = context;
        }


       
        [HttpGet]
        public async Task<IActionResult> GetAll(int userId, int lastDays = 7)
        {
            var result = await _context.GoalRecords
                .Where(x => x.UserId == userId && x.Date >= DateTime.Today.AddDays(-lastDays))
                .Select(x => new GoalRecordListDto
                {
                    Id = x.GoalRecordId,
                    Note = x.Note,
                    Date = x.Date,
                    GoalTitle = x.Goal.Title
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GoalCreateDto dto)
        {
            var goal = new Goal
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                Period = dto.Period,
                Importance = dto.Importance
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return Ok();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
                return NotFound();

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
