using AppointmentService.Application.Helpers;
using AppointmentService.Domain;
using AppointmentService.Persistence.Context;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentService.Application.Appointments;

public class Create
{
    public class Command : IRequest<OperationResult<Unit>>
    {
        public AppointmentDto Appointment { get; set; } = null!;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Appointment).SetValidator(new AppointmentValidator());
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
            var master = await _context.Masters
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Appointment.MasterId, cancellationToken);
            
            if (master == null) return OperationResult<Unit>.Failure("Мастер не существует");

            var appointmentExisted = await _context.Appointments
                .FirstOrDefaultAsync(x => x.MasterId == master.Id 
                                          && x.Start < request.Appointment.End 
                                          && x.End > request.Appointment.Start, cancellationToken: cancellationToken);

            if (appointmentExisted != null) return OperationResult<Unit>.Failure("Конфликт бронирования. Выберите другой временной слот.");

            var appointment = _mapper.Map<Appointment>(request.Appointment);

            await _context.Appointments.AddAsync(appointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}