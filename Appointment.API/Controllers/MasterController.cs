using Appointment.Application.Helpers;
using Appointment.Application.Masters;
using Appointment.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.API.Controllers;

public class MasterController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<OperationResult<Master>>> GetMaster(string id)
    {
        return Ok(await Mediator.Send(new Details.Query {Id = id}));
    }
}