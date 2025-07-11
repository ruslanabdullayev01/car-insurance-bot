using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers.Base;

public class CustomBaseController : ControllerBase
{
    //#region Actions
    //public ObjectResult NewResult<T>(Result<T> response)
    //{
    //    switch (response.statusCode)
    //    {
    //        case 200:
    //            return new OkObjectResult(response);
    //        case 201:
    //            return new CreatedResult(string.Empty, response);
    //        case 401:
    //            return new UnauthorizedObjectResult(response);
    //        case 400:
    //            return new BadRequestObjectResult(response);
    //        case 404:
    //            return new NotFoundObjectResult(response);
    //        case 202:
    //            return new AcceptedResult(string.Empty, response);
    //        case 422:
    //            return new UnprocessableEntityObjectResult(response);
    //        default:
    //            return new BadRequestObjectResult(response);
    //    }
    //}
    //#endregion

    [NonAction]
    public IActionResult CreateActionResult<T>(Result<T> response)
    {
        return new ObjectResult(response)
        {
            StatusCode = response.statusCode
        };
    }
}
