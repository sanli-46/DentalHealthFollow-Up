using System;
namespace DentalHealthFollow_Up.Shared.DTOs
{
    public class GoalRecordCreateDto
    {
        public int UserId { get; set; }
        public int GoalId { get; set; }
        public DateTime Date { get; set; }
        public int? DurationInMinutes { get; set; }
        public string? Note { get; set; }
        public string? ImageBase64 { get; set; }
    }
}


