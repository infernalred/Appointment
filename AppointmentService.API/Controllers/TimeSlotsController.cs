using System.Net;
using AppointmentService.Application.Helpers;
using AppointmentService.Application.TimeSlots;
using AppointmentService.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class TimeSlotsController : BaseApiController
{
    [Authorize(Roles = $"{Roles.Master}")]
    [HttpGet]
    [ProducesResponseType(typeof(OperationResult<TimeSlotDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<TimeSlotDto>>> GetTimeSlots()
    {
        return Ok(await Mediator.Send(new List.Query()));
    }
}