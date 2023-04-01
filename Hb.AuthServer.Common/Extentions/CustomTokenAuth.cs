using Hb.AuthServer.Common.Options;
using Hb.AuthServer.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Common.Extentions
{
    public static class CustomTokenAuth
    {

        public static void AddTokenAuth(this IServiceCollection services,CustomTokenOptions tokenOptions)
        {

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions!.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSecurityKey(tokenOptions.SecurityKey),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                };




                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        }
    }

}
