using AppointmentService.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => (_mediator ??= HttpContext.RequestServices.GetService<IMediator>())!;

    protected ActionResult HandleResult<T>(OperationResult<T> result)
    {
        return result.IsSuccess switch
        {
            true when result.Result != null => Ok(result),
            true when result.Result == null => NotFound(),
            _ => BadRequest(result)
        };
    }
}