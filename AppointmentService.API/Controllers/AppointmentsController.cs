using System.Net;
using AppointmentService.Application.Appointments;
using AppointmentService.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class AppointmentsController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(OperationResult<AppointmentDto>), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<OperationResult<AppointmentDto>>> CreateAppointment(AppointmentDto appointment)
    {
        var result = await Mediator.Send(new Create.Command {Appointment = appointment});

        if (result.IsSuccess)
        {
            return CreatedAtRoute(nameof(GetAppointment), new {id = appointment.Id},
                OperationResult<AppointmentDto>.Success(appointment));
        }

        return Conflict(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetAppointment))]
    [ProducesResponseType(typeof(OperationResult<AppointmentDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<AppointmentDto>>> GetAppointment(Guid id)
    {
        return Ok(await Mediator.Send(new Details.Query {Id = id}));
    }
}