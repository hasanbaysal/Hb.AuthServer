using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hb.AuthServer.API.Controllers
{
    [Route("api/[controller]")]

    public class UserController : CustomBaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var result=  await userService.CreateUserAsync(dto); 

            return ActionResultInstance(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserByName()
        {
            return ActionResultInstance(await userService.GetUserByNameAsync(HttpContext.User.Identity!.Name!));
        }

        [HttpPost("CreateUserRole/{userName}")]
        public async Task<IActionResult> CreateUserRole(string userName)
        {
          return ActionResultInstance(await userService.CreateUserRole(userName));
        }
    }
}
