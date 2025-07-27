using CarInsuranceBot.Application.MediatR.Queries.Conversation;
using Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers
{
    [Route("api/conversations")]
    [ApiController]
    public class ConversationsController(IMediator mediator) : CustomBaseController
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetConversationsByUserId([FromRoute] string userId, [FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetConversationsByUserIdQuery(userId, paginationQuery.Page, paginationQuery.PageSize), cancellationToken);
            return Ok(result);
        }
    }
}