namespace Appointment.Domain;

public class Appointment
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string MasterId { get; set; } = string.Empty;
    public string? Master { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}