namespace DentalHealthFollow_Up.Shared.DTOs;

public class PasswordResetDto
{
   
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Code { get; set; } = null!;
}
