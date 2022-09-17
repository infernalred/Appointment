using AppointmentService.Persistence.Context;
using AppointmentService.Application.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Masters;

public class List
{
    public class Query : IRequest<OperationResult<List<MasterDto>>>{}
    
    public class Handler : IRequestHandler<Query, OperationResult<List<MasterDto>>>
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
        
        public async Task<OperationResult<List<MasterDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var masters = await _context.Masters
                .Include(x => x.User)
                .OrderBy(x => x.ServiceId)
                .ProjectTo<MasterDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            return OperationResult<List<MasterDto>>.Success(masters);
        }
    }
}