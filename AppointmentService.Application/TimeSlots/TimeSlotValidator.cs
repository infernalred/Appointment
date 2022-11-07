using FluentValidation;

namespace AppointmentService.Application.TimeSlots;

public class TimeSlotValidator : AbstractValidator<TimeSlotDto>
{
    public TimeSlotValidator()
    {
        RuleFor(x => x.End).GreaterThan(x => x.Start);
    }
}