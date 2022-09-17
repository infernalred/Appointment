using System.Net;
using AppointmentService.Application.Appointments;
using AppointmentService.Application.Helpers;
using AppointmentService.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class AppointmentController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OperationResult<List<Slot>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<List<Slot>>>> GetFreeSlots(string id,
        [FromQuery] SlotParams slotParams)
    {
        return Ok(await Mediator.Send(new FreeSlots.Query {Id = id, Params = slotParams}));
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<OperationResult<Unit>>> CreateAppointment(AppointmentDto appointment)
    {
        var result = await Mediator.Send(new Create.Command {Appointment = appointment});

        if (result.IsSuccess)
        {
            return CreatedAtRoute(nameof(Appointment), new {id = appointment.Id}, result);
        }

        return Ok(result);
    }
}