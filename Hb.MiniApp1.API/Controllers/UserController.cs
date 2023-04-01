using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hb.MiniApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetUserİnfo()
        {
            var userName = HttpContext.User.Identity!.Name;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return Ok($"miniapi1 user name => {userName} user id=> {userId}");
        }
    }
}
