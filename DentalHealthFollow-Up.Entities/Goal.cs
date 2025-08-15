using System;
using System.Collections.Generic;
using System.Text;

namespace DentalHealthFollow_Up.Entities
{
    public class Goal
    {
        public int GoalId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Importance { get; set; } = null!;


        public User User { get; set; } = null!;
        public ICollection<GoalRecord> GoalRecords { get; set; } = new List<GoalRecord>();
    
    
    }
}
