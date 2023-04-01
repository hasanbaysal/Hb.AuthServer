using Hb.AuthServer.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hb.AuthServer.API.Controllers
{

    [ApiController]
    public class CustomBaseController : ControllerBase
    {

        [NonAction]
        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCoe
            };



        }

    }
}
