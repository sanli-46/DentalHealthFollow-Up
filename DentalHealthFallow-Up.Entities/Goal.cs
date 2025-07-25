using System;
using System.Collections.Generic;
using System.Text;

namespace DentalHealthFallow_Up.Entities
{
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Importance { get; set; } = null!;


        public User User { get; set; } = null!;
        public ICollection<GoalRecord> GoalRecords { get; set; } = new List<GoalRecord>();
    
    
    }
}
