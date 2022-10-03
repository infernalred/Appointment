using FluentValidation;

namespace AppointmentService.Application.Masters;

public class SlotParamsValidator : AbstractValidator<SlotParams>
{
    public SlotParamsValidator()
    {
        RuleFor(x => x.QuantityDays).LessThanOrEqualTo(28);
        //add to check start date
    }
}