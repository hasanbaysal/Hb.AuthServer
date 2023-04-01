using Hb.AuthServer.Core.Configuration;
using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp user);
        ClientTokenDto CreateTokenByClient(Client client);


    }
}
