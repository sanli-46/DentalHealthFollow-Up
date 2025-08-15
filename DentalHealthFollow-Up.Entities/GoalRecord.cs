using System;

namespace DentalHealthFollow_Up.Entities
{
    public class GoalRecord
    {
        public int GoalRecordId { get; set; }
        public int UserId { get; set; }
        public int GoalId { get; set; }

        public DateTime Date { get; set; }
        public int? DurationInMinutes { get; set; } // nullable yapıldı
        public string? Note { get; set; }
        public string? ImageBase64 { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
        public Goal Goal { get; set; } = null!;
    }
}
