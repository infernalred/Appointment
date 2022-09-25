using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Services;

public class List
{
    public class Query : IRequest<OperationResult<List<ServiceDto>>>{}

    public class Handler : IRequestHandler<Query, OperationResult<List<ServiceDto>>>
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

        public async Task<OperationResult<List<ServiceDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var services = await _context.Services
                .Include(x => x.Masters)
                .ThenInclude(m => m.User)
                .OrderBy(x => x.Id)
                .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return OperationResult<List<ServiceDto>>.Success(services);
        }
    }
}