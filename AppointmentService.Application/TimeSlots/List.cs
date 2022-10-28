using AppointmentService.Application.Contracts;
using AppointmentService.Application.Helpers;
using AppointmentService.Domain;
using AppointmentService.Persistence.Context;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.TimeSlots;

public class List
{
    public class Query : IRequest<OperationResult<List<TimeSlotDto>>>
    {
    }

    public class Handler : IRequestHandler<Query, OperationResult<List<TimeSlotDto>>>
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

        public async Task<OperationResult<List<TimeSlotDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

            var slots = await _context.TimeSlots
                .Where(x => x.MasterId == user.Id)
                .ProjectTo<TimeSlotDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return OperationResult<List<TimeSlotDto>>.Success(slots);
        }
    }
}