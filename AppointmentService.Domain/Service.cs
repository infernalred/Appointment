namespace AppointmentService.Domain;

public class Service
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Image { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public float Price { get; set; }
    public ICollection<Master> Masters { get; set; } = new List<Master>();
}