using System;

public class GoalRecordListDto
{
    public int Id { get; set; }
    public string Note { get; set; } = "";
    public string GoalTitle { get; set; } = "";
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
}
