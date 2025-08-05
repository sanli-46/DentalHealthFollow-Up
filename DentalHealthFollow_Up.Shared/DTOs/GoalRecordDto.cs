using System;

namespace DentalHealthFollow_Up.Shared.DTOs
{
    public class GoalRecordDto
    {
        public int Id { get; set; }

        public int GoalId { get; set; }

        public DateTime Date { get; set; }
      


        public TimeSpan Time { get; set; }

        public int DurationInMinutes { get; set; }

        public string Note { get; set; } = string.Empty;

        public string? ImageBase64 { get; set; }

        public int UserId { get; set; }
       
    }
}
