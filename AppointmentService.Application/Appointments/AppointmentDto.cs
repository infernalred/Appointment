using AppointmentService.Application.Masters;

namespace AppointmentService.Application.Appointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string MasterId { get; set; } = null!;
    public MasterDto? Master { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}