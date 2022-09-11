namespace Appointment.Domain;

public class Service
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Image { get; set; }
    public bool IsEnabled { get; set; } = true;
}