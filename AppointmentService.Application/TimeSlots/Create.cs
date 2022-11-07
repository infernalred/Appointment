using AppointmentService.Application.Contracts;
using AppointmentService.Application.Helpers;
using AppointmentService.Domain;
using AppointmentService.Persistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.TimeSlots;

public class Create
{
    public class Command : IRequest<OperationResult<Unit>>
    {
        public TimeSlotDto TimeSlot { get; set; } = null!;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.TimeSlot).SetValidator(new TimeSlotValidator());
        }
    }

    public class Handler : IRequestHandler<Command, OperationResult<Unit>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(ILogger<Handler> logger, DataContext context, IUserAccessor userAccessor)
        {
            _logger = logger;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<OperationResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            var slots = await _context.TimeSlots
                .Where(x => x.MasterId == user.Id
                            && x.DayOfWeek == request.TimeSlot.DayOfWeek)
                .ToListAsync(cancellationToken);

            if (slots.Any(x => x.Start.TimeOfDay < request.TimeSlot.End.TimeOfDay
                               && x.End.TimeOfDay > request.TimeSlot.Start.TimeOfDay))
            {
                return OperationResult<Unit>.Failure("Время не должно пересекаться");
            }

            var slot = new TimeSlot
            {
                Id = request.TimeSlot.Id,
                DayOfWeek = request.TimeSlot.DayOfWeek,
                Start = request.TimeSlot.Start,
                End = request.TimeSlot.End,
                MasterId = user.Id
            };

            _context.TimeSlots.Add(slot);
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}