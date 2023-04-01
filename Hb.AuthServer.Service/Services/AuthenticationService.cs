using Hb.AuthServer.Common.Dtos;
using Hb.AuthServer.Core.Configuration;
using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Entity;
using Hb.AuthServer.Core.Repositories;
using Hb.AuthServer.Core.Services;
using Hb.AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> clients;
        private readonly ITokenService tokenService;
        private readonly UserManager<UserApp> userManager;
        private readonly IUnitOfWork unitOfWork; 
        private readonly IGenericRepository<UserRefreshToken> userRefreshTokenRepository;

        public AuthenticationService(IOptions<List<Client>> options,ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRepository)
        {
            this.clients = options.Value;
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.userRefreshTokenRepository = userRepository;
        }

        public async Task<Response<TokenDto>> CreateToken(LoginDto dto)
        {
            if (dto == null) throw new Exception("data empty");

            var user = await userManager.FindByEmailAsync(dto.Email);


            if (user == null) return Response<TokenDto>.Fail(new("user name or password is wrong", true), 404);


            if(!(await userManager.CheckPasswordAsync(user, dto.Password))) return Response<TokenDto>.Fail(new("user name or password is wrong", true), 404);

            var token = tokenService.CreateToken(user);

            var userRefreshToken = await userRefreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
               await userRefreshTokenRepository.AddAsync(new UserRefreshToken() { UserId = user.Id, CodeRefresh = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.CodeRefresh = token.RefreshToken;
                userRefreshToken.Expiration=token.RefreshTokenExpiration;

            }

            await unitOfWork.CommitAsync();

            return Response<TokenDto>.Succes(token, 200);

        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto dto)
        {

            var client = clients.SingleOrDefault(x => x.Id == dto.ClientId && x.Secret == dto.ClientSecretID);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("client id or secret is wrong", 404, true);
            }

            var token = tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Succes(token, 200);


        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {

            var refreshTokenIsExist = await userRefreshTokenRepository.Where(x => x.CodeRefresh == refreshToken).SingleOrDefaultAsync();

            if (refreshTokenIsExist == null)
            {
                return Response<TokenDto>.Fail("refresh token not found", 404, true);
            }

            var user = await userManager.FindByIdAsync(refreshTokenIsExist.UserId);

            if (user == null)
            {
                return Response<TokenDto>.Fail("user id not found", 404, true);
            }

            var tokendto = tokenService.CreateToken(user);

            refreshTokenIsExist.CodeRefresh = tokendto.RefreshToken;
            refreshTokenIsExist.Expiration= tokendto.RefreshTokenExpiration;


            await unitOfWork.CommitAsync();

            return Response<TokenDto>.Succes(tokendto, 200);

        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var refreshTokenIsExist = await userRefreshTokenRepository.Where(x => x.CodeRefresh == refreshToken).SingleOrDefaultAsync();


            if (refreshTokenIsExist == null)
            {
                return Response<NoDataDto>.Fail("refresh token not found", 404, true);
            }

            userRefreshTokenRepository.Remove(refreshTokenIsExist);
            await unitOfWork.CommitAsync();

            return Response<NoDataDto>.Succes(200);

        }
    }
}
