using CarInsuranceBot.Application.MediatR.Queries.GetExtractedField;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers
{
    [Route("api/extracted-fields")]
    [ApiController]
    public class ExtractedFieldsController(IMediator mediator) : CustomBaseController
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetExtractedFieldsByUserId([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetExtractedFieldsByUserIdQuery(userId), cancellationToken);
            return Ok(result);
        }
    }
}
