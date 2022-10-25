using AppointmentService.Application.Masters;

namespace AppointmentService.Application.Services;

public class ServiceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public string? Image { get; set; }
    public bool IsEnabled { get; set; }
    public ICollection<MasterDto> Masters { get; set; } = new List<MasterDto>();
}