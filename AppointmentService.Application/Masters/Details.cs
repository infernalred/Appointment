using AppointmentService.Persistence.Context;
using AppointmentService.Application.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Masters;

public class Details
{
    public class Query : IRequest<OperationResult<MasterDto>>
    {
        public string Id { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Query, OperationResult<MasterDto>>
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
        
        public async Task<OperationResult<MasterDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var master = await _context.Masters
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return OperationResult<MasterDto>.Success(_mapper.Map<MasterDto>(master));
        }
    }
}