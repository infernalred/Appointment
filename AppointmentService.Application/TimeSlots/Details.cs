using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.TimeSlots;

public class Details
{
    public class Query : IRequest<OperationResult<TimeSlotDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, OperationResult<TimeSlotDto>>
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

        public async Task<OperationResult<TimeSlotDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var slot = await _context.TimeSlots
                .ProjectTo<TimeSlotDto>(_mapper.ConfigurationProvider)
                .FirstAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            return OperationResult<TimeSlotDto>.Success(slot);
        }
    }
}