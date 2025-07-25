using CarInsuranceBot.Application.MediatR.Queries.Policy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers
{
    [Route("api/policies")]
    [ApiController]
    public class PoliciesController(IMediator mediator) : CustomBaseController
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPolicyByUserId([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPolicyByUserIdQuery(userId), cancellationToken);
            return Ok(result);
        }
    }
}
