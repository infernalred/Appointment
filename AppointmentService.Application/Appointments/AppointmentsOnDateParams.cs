namespace AppointmentService.Application.Appointments;

public class AppointmentsOnDateParams
{
    public DateTime OnDate { get; set; } = DateTime.UtcNow;
}