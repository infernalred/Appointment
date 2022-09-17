using FluentValidation;

namespace AppointmentService.Application.Appointments;

public class AppointmentValidator : AbstractValidator<AppointmentDto>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.MasterId).NotEmpty();
        RuleFor(x => x.Start).GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.End).GreaterThan(x => x.Start);
    }
}