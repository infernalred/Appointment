using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.TimeSlots;

public class Delete
{
    public class Command : IRequest<OperationResult<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, OperationResult<Unit>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly DataContext _context;

        public Handler(ILogger<Handler> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<OperationResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var slot = await _context.TimeSlots
                .AsTracking()
                .FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            _context.Remove(slot);
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}