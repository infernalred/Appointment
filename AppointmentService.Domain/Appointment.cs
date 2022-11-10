namespace AppointmentService.Domain;

public class Appointment
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string MasterId { get; set; } = null!;
    public Master? Master { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}