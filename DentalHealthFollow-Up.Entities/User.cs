using System;
using System.Collections.Generic;
using System.Text;

namespace DentalHealthFollow_Up.Entities
{
   public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } =string.Empty;
        public string Salt { get; set; } = string.Empty;
        public DateTime birthdate{ get; set; }


        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<GoalRecord> GoalRecords { get; set; } = new List<GoalRecord>();
        public ICollection<PasswordReset> PasswordResets { get; set; }  = new List<PasswordReset>();
    }
}
