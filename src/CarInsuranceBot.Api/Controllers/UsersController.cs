using CarInsuranceBot.Application.MediatR.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IMediator mediator) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
            Ok(await mediator.Send(new GetUsersQuery(), cancellationToken));
    }
}
