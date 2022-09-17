using AppointmentService.Application.Masters;

namespace AppointmentService.Application.Appointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string MasterId { get; set; } = string.Empty;
    public MasterDto? Master { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}