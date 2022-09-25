using System.Net;
using AppointmentService.Application.Helpers;
using AppointmentService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class ServiceController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OperationResult<ServiceDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<ServiceDto>>> GetService(int id)
    {
        return Ok(await Mediator.Send(new Details.Query {Id = id}));
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(OperationResult<List<ServiceDto>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<List<ServiceDto>>>> GetServices()
    {
        return Ok(await Mediator.Send(new List.Query()));
    }
}