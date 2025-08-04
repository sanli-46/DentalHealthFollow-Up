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
        public async Task<IActionResult> GetAll([FromQuery] int userId)
        {
            var goals = await _context.Goals
                .Where(g => g.UserId == userId)
                .Select(g => new GoalDto
                {
                    Id = g.Id,
                    UserId = g.UserId,
                    Title = g.Title,
                    Description = g.Description,
                    Period = g.Period,
                    Importance = g.Importance
                })
                .ToListAsync();

            return Ok(goals);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GoalDto dto)
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

            dto.Id = goal.Id;
            return Ok(dto);
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
