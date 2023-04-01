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
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
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

        public async Task<Response<NoDataDto>> CreateUserRole(string userName)
        {

            if ( !await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new() { Name = "admin" });
                await roleManager.CreateAsync(new() { Name = "manager" });
            }

            var user = await userManager.FindByNameAsync(userName);

            await userManager.AddToRoleAsync(user, "admin");
            await userManager.AddToRoleAsync(user, "manager");

            return Response<NoDataDto>.Succes(200);
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
