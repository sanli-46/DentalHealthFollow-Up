using System;
using System.Collections.Generic;
using System.Text;

namespace DentalHealthFallow_Up.Entities
{
    public class GoalRecord
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int GoalId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Note { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }



        public User User { get; set; } = null!;
        public Goal Goal { get; set; } = null!;
    }
}
