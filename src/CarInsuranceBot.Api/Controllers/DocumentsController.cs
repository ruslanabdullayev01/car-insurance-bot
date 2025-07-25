using CarInsuranceBot.Application.MediatR.Queries.Document;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController(IMediator mediator) : CustomBaseController
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDocumentsByUserId([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetDocumentsByUserIdQuery(userId), cancellationToken);
            return Ok(result);
        }
    }
}
