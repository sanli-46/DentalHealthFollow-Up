using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DentalHealthFallow_Up.Entities
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }



        public User User { get; set; } = null!;
    }
}
