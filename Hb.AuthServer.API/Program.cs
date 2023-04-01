using Hb.AuthServer.Common.Extentions;
using Hb.AuthServer.Common.Options;
using Hb.AuthServer.Core.Configuration;
using Hb.AuthServer.Core.Entity;
using Hb.AuthServer.Core.Repositories;
using Hb.AuthServer.Core.Services;
using Hb.AuthServer.Core.UnitOfWork;
using Hb.AuthServer.Data.Context;
using Hb.AuthServer.Data.Repositories;
using Hb.AuthServer.Data.UnitOfWork;
using Hb.AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));


builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitofWork>();


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), sqlopt =>
    {
        sqlopt.MigrationsAssembly("Hb.AuthServer.Data");
    });
});


builder.Services.AddIdentity<UserApp, IdentityRole>(opt =>
{

    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;


}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


//builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();



#region  extentions

//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


//}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
//{

//    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//    {
//        ValidIssuer = tokenOptions!.Issuer,
//        ValidAudience = tokenOptions.Audience[0],
//        IssuerSigningKey = SignService.GetSecurityKey(tokenOptions.SecurityKey),
//        ValidateIssuerSigningKey = true,
//        ValidateAudience=true,
//        ValidateIssuer=true,
//        ValidateLifetime=true,
//        ClockSkew=TimeSpan.Zero

//    };




//    opt.Events = new JwtBearerEvents
//    {
//        OnAuthenticationFailed = context =>
//        {
//            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//            {
//                context.Response.Headers.Add("Token-Expired", "true");
//                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//            }

//            return Task.CompletedTask;
//        }
//    };
//});

#endregion


builder.Services.AddTokenAuth(tokenOptions!);






var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
