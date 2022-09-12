namespace Appointment.Domain;

public class Appointment
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string MasterId { get; set; } = string.Empty;
    public Master? Master { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}