using FluentValidation;

namespace AppointmentService.Application.Masters;

public class SlotParamsValidator : AbstractValidator<SlotParams>
{
    public SlotParamsValidator()
    {
        RuleFor(x => x.QuantityDays).InclusiveBetween(0, 6);
        RuleFor(x => x.Start.Date).GreaterThanOrEqualTo(DateTime.UtcNow.Date);
    }
}