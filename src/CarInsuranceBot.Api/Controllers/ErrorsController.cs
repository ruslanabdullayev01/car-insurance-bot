using CarInsuranceBot.Application.MediatR.Queries.Error;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/errors")]
    [ApiController]
    public class ErrorController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllErrors(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetErrorsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetErrorsByUserId([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetErrorsByUserIdQuery(userId), cancellationToken);
            return Ok(result);
        }
    }
}
