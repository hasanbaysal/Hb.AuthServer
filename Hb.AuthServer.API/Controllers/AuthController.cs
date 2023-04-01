using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hb.AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]

  
    public class AuthController : CustomBaseController
    {

        private readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await  authenticationService.CreateToken(loginDto);

            return ActionResultInstance(result);


        }

        [HttpPost]
        public  IActionResult CreateTokenByClient( ClientLoginDto loginDto)
        {

            var result =  authenticationService.CreateTokenByClient(loginDto);

            return ActionResultInstance(result);

        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await authenticationService.RevokeRefreshToken(refreshTokenDto.RefToken);

            return ActionResultInstance(result);

        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto dto)
        {
            var result = await authenticationService.CreateTokenByRefreshToken(dto.RefToken);

            return ActionResultInstance(result);

        }
    }
}
