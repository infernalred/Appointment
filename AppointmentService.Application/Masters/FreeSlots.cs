using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
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
            var result = new List<Slot>();

            var master = await _context.Masters
                .Include(x => x.TimeSlots)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (master == null) return OperationResult<List<Slot>>.Success(result);

            master.Service = await _context.Services.FirstAsync(x => x.Id == master.ServiceId, cancellationToken);

            var appointments = await _context.Appointments
                .Where(x => x.MasterId == master.Id && x.Start > request.Params.Start && x.End < request.Params.End)
                .OrderBy(x => x.Start)
                .ToListAsync(cancellationToken);

            var daysCount = request.Params.End.Day - request.Params.Start.Day;

            var start = request.Params.Start;
            var end = new DateTime(start.Year, start.Month, start.Day + 1, 0, 0, 0, start.Kind);

            for (var i = 0; i <= daysCount; i++)
            {
                var slots = master.TimeSlots
                    .OrderBy(x => x.Start)
                    .Where(x => x.DayOfWeek == start.DayOfWeek).ToList();

                foreach (var slot in slots)
                {
                    var startDateTime = new DateTime(start.Year, start.Month, start.Day, slot.Start.Hour,
                        slot.Start.Minute, 0, start.Kind);

                    var newSlot = new Slot
                    {
                        Id = Guid.NewGuid(),
                        Start = startDateTime,
                        End = startDateTime.AddMinutes(master.Service.DurationMinutes)
                    };

                    if (!appointments.Any(x => x.Start < newSlot.End && x.End > newSlot.Start))
                    {
                        result.Add(newSlot);
                    }
                }

                start = end;
                end = end.AddDays(1);
            }

            return OperationResult<List<Slot>>.Success(result);
        }
    }
}