using System;

namespace DentalHealthFollow_Up.Shared.DTOs
{
    public class UserUpdateDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
