using System.Collections.Concurrent;
using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Masters;

public class FreeSlots
{
    public class Query : IRequest<OperationResult<List<Slot>>>
    {
        public string Id { get; set; } = null!;
        public SlotParams Params { get; set; } = null!;
    }

    public class CommandValidator : AbstractValidator<Query>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Params).SetValidator(new SlotParamsValidator());
        }
    }

    public class Handler : IRequestHandler<Query, OperationResult<List<Slot>>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly DataContext _context;

        public Handler(ILogger<Handler> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<OperationResult<List<Slot>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = new ConcurrentBag<Slot>();

            var master = await _context.Masters
                .Include(x => x.TimeSlots)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (master == null) return OperationResult<List<Slot>>.Success(result.ToList());

            master.Service = await _context.Services.FirstAsync(x => x.Id == master.ServiceId, cancellationToken);

            var startEndTime = request.Params.Start;
            var endDateTime = new DateTime(startEndTime.Year, startEndTime.Month, startEndTime.Day, 0, 0, 0,
                startEndTime.Kind);
            endDateTime = endDateTime.AddDays(request.Params.QuantityDays);

            var appointments = await _context.Appointments
                .Where(x => x.MasterId == master.Id && x.Start > request.Params.Start && x.End < endDateTime)
                .OrderBy(x => x.Start)
                .ToListAsync(cancellationToken);

            Parallel.For(0, request.Params.QuantityDays + 1, i =>
            {
                var start = request.Params.Start.AddDays(i);

                var slots = master.TimeSlots
                    .OrderBy(x => x.Start)
                    .Where(x => x.DayOfWeek == start.DayOfWeek);

                foreach (var slot in slots)
                {
                    var minutes = (slot.End - slot.Start).TotalMinutes;

                    var startDateTime = new DateTime(start.Year, start.Month, start.Day, slot.Start.Hour,
                        slot.Start.Minute, 0, start.Kind);

                    while (minutes >= master.Service.DurationMinutes)
                    {
                        var newSlot = new Slot
                        {
                            Id = Guid.NewGuid(),
                            Start = startDateTime,
                            End = startDateTime.AddMinutes(master.Service.DurationMinutes)
                        };

                        if (newSlot.Start > DateTime.UtcNow &&
                            !appointments.Any(x => x.Start < newSlot.End && x.End > newSlot.Start))
                        {
                            result.Add(newSlot);
                        }

                        startDateTime = startDateTime.AddMinutes(master.Service.DurationMinutes);
                        minutes -= master.Service.DurationMinutes;
                    }
                }
            });

            return OperationResult<List<Slot>>.Success(result.ToList());
        }
    }
}