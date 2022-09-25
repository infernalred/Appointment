using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Services;

public class Details
{
    public class Query : IRequest<OperationResult<ServiceDto>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, OperationResult<ServiceDto>>
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

        public async Task<OperationResult<ServiceDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var service = await _context.Services
                .Include(x => x.Masters)
                .ThenInclude(m => m.User)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return OperationResult<ServiceDto>.Success(_mapper.Map<ServiceDto>(service));
        }
    }
}