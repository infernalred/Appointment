using System.Net;
using AppointmentService.Application.Services;
using AppointmentService.Application.Helpers;
using AppointmentService.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

public class ServicesController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetService))]
    [ProducesResponseType(typeof(OperationResult<ServiceDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<ServiceDto>>> GetService(Guid id)
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

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<ServiceDto>), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ServiceDto>> CreateService(ServiceDto service)
    {
        await Mediator.Send(new Create.Command {Service = service});
        return CreatedAtRoute(nameof(GetService), new {id = service.Id},
            OperationResult<ServiceDto>.Success(service));
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<OperationResult<Unit>>> EditService(Guid id, ServiceDto service)
    {
        service.Id = id;
        var result = await Mediator.Send(new Edit.Command {Service = service});

        return Ok(result);
    }
}