using AppointmentService.Application.Contracts;
using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.TimeSlots;

public class Edit
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
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(ILogger<Handler> logger, DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<OperationResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            var slots = await _context.TimeSlots
                .Where(x => x.MasterId == user.Id
                            && x.DayOfWeek == request.TimeSlot.DayOfWeek
                            && x.Id != request.TimeSlot.Id)
                .ToListAsync(cancellationToken);

            if (slots.Any(x => x.Start.TimeOfDay < request.TimeSlot.End.TimeOfDay
                               && x.End.TimeOfDay > request.TimeSlot.Start.TimeOfDay))
            {
                return OperationResult<Unit>.Failure("Время не должно пересекаться");
            }

            var slot = await _context.TimeSlots
                .AsTracking()
                .FirstAsync(x => x.Id == request.TimeSlot.Id, cancellationToken);

            _mapper.Map(request.TimeSlot, slot);

            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}