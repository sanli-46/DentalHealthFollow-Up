namespace DentalHealthFollow_Up.Shared.DTOs;
public class GoalCreateDto
{
    public int UserId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Period { get; set; } = "";
    public string Importance { get; set; } = "";
}


