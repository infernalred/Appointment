using AppointmentService.Application.Contracts;
using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Appointments;

public class MyAppointmentsByDate
{
    public class Query : IRequest<OperationResult<List<AppointmentDto>>>
    {
    }

    public class Handler : IRequestHandler<Query, OperationResult<List<AppointmentDto>>>
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
        
        public async Task<OperationResult<List<AppointmentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstAsync(x => x.UserName == _userAccessor.GetUsername(),
                    cancellationToken: cancellationToken);

            var date = DateTime.UtcNow.Date;
            var appointments = await _context.Appointments
                .Where(x => x.MasterId == user.Id && x.Start > date && x.End < date.AddDays(1))
                .OrderBy(x => x.Start)
                .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            return OperationResult<List<AppointmentDto>>.Success(appointments);
        }
    }
}