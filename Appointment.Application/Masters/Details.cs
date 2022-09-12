using Appointment.Application.Helpers;
using Appointment.Domain;
using Appointment.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Appointment.Application.Masters;

public class Details
{
    public class Query : IRequest<OperationResult<Master>>
    {
        public string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, OperationResult<Master>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly DataContext _context;

        public Handler(ILogger<Handler> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<OperationResult<Master>> Handle(Query request, CancellationToken cancellationToken)
        {
            var master = await _context.Masters
                .Include(x => x.Service)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            
            return OperationResult<Master>.Success(master);
        }
    }
}