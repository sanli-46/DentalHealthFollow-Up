namespace DentalHealthFollow_Up.Shared.DTOs
{
    public class GoalDto
    {
        public int GoalId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Period { get; set; } = string.Empty;

        public string Importance { get; set; } = string.Empty;
    }
}
