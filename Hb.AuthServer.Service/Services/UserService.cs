using Hb.AuthServer.Common.Dtos;
using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Entity;
using Hb.AuthServer.Core.Services;
using Hb.AuthServer.Service.Mappings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hb.AuthServer.Service.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<UserApp> userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto dto)
        {
           
            var user = new UserApp() { Email=dto.Email,UserName=dto.UserName, City="default"};

            var result = await userManager.CreateAsync(user,dto.Password);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new(erros, true), 400);
            }

            return Response<UserAppDto>.Succes(ObjectMapper.Mapper.Map<UserAppDto>(user),200);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            
            if (user == null)
            {
                return Response<UserAppDto>.Fail("user not found", 400,true);
            }

            return Response<UserAppDto>.Succes(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
