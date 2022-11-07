using System.Net;
using AppointmentService.Application.Helpers;
using AppointmentService.Application.TimeSlots;
using AppointmentService.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class TimeSlotsController : BaseApiController
{
    [Authorize(Roles = $"{Roles.Master}")]
    [HttpGet]
    [ProducesResponseType(typeof(OperationResult<List<TimeSlotDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<OperationResult<List<TimeSlotDto>>>> GetTimeSlots()
    {
        return Ok(await Mediator.Send(new List.Query()));
    }
    
    [Authorize(Policy = "IsSlotOwner")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<TimeSlotDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<OperationResult<TimeSlotDto>>> GetSlot(Guid id)
    {
        return Ok(await Mediator.Send(new Details.Query{Id = id}));
    }

    [Authorize(Roles = $"{Roles.Master}")]
    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<OperationResult<Unit>>> CreateSlot(TimeSlotDto slot)
    {
        return Ok(await Mediator.Send(new Create.Command {TimeSlot = slot}));
    }
    
    [Authorize(Policy = "IsSlotOwner")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<OperationResult<Unit>>> EditSlot(Guid id, TimeSlotDto slot)
    {
        slot.Id = id;
        return Ok(await Mediator.Send(new Edit.Command {TimeSlot = slot}));
    }

    [Authorize(Policy = "IsSlotOwner")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<OperationResult<Unit>>> DeleteSlot(Guid id)
    {
        return Ok(await Mediator.Send(new Delete.Command {Id = id}));
    }
}