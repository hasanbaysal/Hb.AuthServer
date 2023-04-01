using Hb.AuthServer.Common.Options;
using Hb.AuthServer.Common.Services;
using Hb.AuthServer.Core.Configuration;
using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Entity;
using Hb.AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Hb.AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<UserApp> userManager;
        private readonly CustomTokenOptions tokenOptions;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> options)
        {
            tokenOptions = options.Value;
            this.userManager = userManager;
        }


        private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences)
        {
         
            var userList = new List<Claim>()
            {

                new  Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new  Claim(JwtRegisteredClaimNames.Email, userApp.Email!),
                new  Claim(ClaimTypes.Name, userApp.UserName!),
                new  Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;

        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();

            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            claims.Add( new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add( new Claim(JwtRegisteredClaimNames.Sub, client.Id));

            return claims;
        }

        private string CreateRefreshToken()
        {

            var numbetByte = new byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numbetByte);

            return Convert.ToBase64String(numbetByte);

        }



        public TokenDto CreateToken(UserApp user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.RefreshTokenExpiration);

            var securirtyKEy = SignService.GetSecurityKey(tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securirtyKEy, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new
                JwtSecurityToken(
                    
                    issuer: tokenOptions.Issuer,
                    expires: accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims : GetClaims(user,tokenOptions.Audience),
                    signingCredentials:signingCredentials

                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

           var token=  handler.WriteToken(jwtSecurityToken);


            var tokenDto = new TokenDto()
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration,
            };

            return tokenDto;


        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);
       

            var securirtyKEy = SignService.GetSecurityKey(tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securirtyKEy, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new
                JwtSecurityToken(

                    issuer: tokenOptions.Issuer,
                    expires: accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims: GetClaimsByClient(client),
                    signingCredentials: signingCredentials

                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);


            var tokenDto = new ClientTokenDto()
            {
                AccessToken = token,
               AccessTokenExpiration = accessTokenExpiration,
             };

            return tokenDto;


        }
    }
}
