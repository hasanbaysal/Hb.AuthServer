using Hb.AuthServer.Common.Dtos;
using Hb.AuthServer.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Core.Services
{
    //bu işlemin repositroy interface'ni yazmadık identity api bize gerekli yapıyı veriyor
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto dto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
     }
}
