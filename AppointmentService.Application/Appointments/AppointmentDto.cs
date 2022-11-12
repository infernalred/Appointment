using AppointmentService.Application.Masters;

namespace AppointmentService.Application.Appointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    private string? _userName;

    public string UserName
    {
        get => _userName ?? string.Empty;
        set => _userName = value.Trim();
    }

    private string? _phone;
    public string Phone
    {
        get => _phone ?? string.Empty;
        set => _phone = value.Trim();
    }
    public string MasterId { get; set; } = null!;
    public MasterDto? Master { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}