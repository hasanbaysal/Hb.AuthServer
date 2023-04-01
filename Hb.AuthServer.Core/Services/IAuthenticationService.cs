using Hb.AuthServer.Common.Dtos;
using Hb.AuthServer.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
       Task<Response<TokenDto>> CreateToken(LoginDto dto);
       Task<Response<TokenDto>> CreateTokenByRefreshToken(string  refreshToken);
       Task<Response<NoDataDto>> RevokeRefreshToken(string  refreshToken); 
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto dto);
       




    }
}
