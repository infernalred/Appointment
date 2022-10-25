using System.Net;
using AppointmentService.Application.Appointments;
using AppointmentService.Application.Helpers;
using AppointmentService.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Create = AppointmentService.Application.Services.Create;
using Details = AppointmentService.Application.Services.Details;

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

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<ServiceDto>), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<ServiceDto>> CreateService(ServiceDto service)
    {
        await Mediator.Send(new Create.Command {Service = service});
        return CreatedAtRoute(nameof(GetService), new {id = service.Id},
            OperationResult<ServiceDto>.Success(service));
    }

    [AllowAnonymous]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<Unit>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OperationResult<Unit>>> CreateService(Guid id, ServiceDto service)
    {
        service.Id = id;
        var result = await Mediator.Send(new Edit.Command {Service = service});

        return Ok(result);
    }
}