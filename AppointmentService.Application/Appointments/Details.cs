using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Appointments;

public class Details
{
    public class Query : IRequest<OperationResult<AppointmentDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, OperationResult<AppointmentDto>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(ILogger<Handler> logger, DataContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(x => x.Master)
                .ThenInclude(m => m!.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return OperationResult<AppointmentDto>.Success(_mapper.Map<AppointmentDto>(appointment));
        }
    }
}