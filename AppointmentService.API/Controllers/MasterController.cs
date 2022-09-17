using System.Net;
using AppointmentService.Application.Helpers;
using AppointmentService.Application.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class MasterController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OperationResult<MasterDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<MasterDto>>> GetMaster(string id)
    {
        return Ok(await Mediator.Send(new Details.Query {Id = id}));
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(OperationResult<List<MasterDto>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<List<MasterDto>>>> GetMasters()
    {
        return Ok(await Mediator.Send(new List.Query()));
    }
}