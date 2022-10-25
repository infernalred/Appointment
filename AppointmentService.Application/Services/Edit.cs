using AppointmentService.Application.Helpers;
using AppointmentService.Persistence.Context;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Services;

public class Edit
{
    public class Command : IRequest<OperationResult<Unit>>
    {
        public ServiceDto Service { get; set; } = null!;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Service).SetValidator(new ServiceValidator());
        }
    }

    public class Handler : IRequestHandler<Command, OperationResult<Unit>>
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

        public async Task<OperationResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var service = await _context.Services
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Service.Id, 
                    cancellationToken: cancellationToken);

            if (service == null) return OperationResult<Unit>.Failure("Услуги не существует");

            _mapper.Map(request.Service, service);

            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}