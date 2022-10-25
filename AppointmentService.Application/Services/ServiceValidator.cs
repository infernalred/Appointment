using FluentValidation;

namespace AppointmentService.Application.Services;

public class ServiceValidator : AbstractValidator<ServiceDto>
{
    public ServiceValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(25);
        RuleFor(x => x.DurationMinutes)
            .GreaterThanOrEqualTo(30)
            .Must(value => value % 30 == 0)
            .WithMessage("Продолжительность должна быть кратна 30");

    }
}