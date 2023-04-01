using Hb.AuthServer.Common.Dtos;
using Hb.AuthServer.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hb.AuthServer.Common.Extentions
{
    public static class CustomExceptionHandler
    {

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {


            app.UseExceptionHandler(configure =>
            {

                configure.Run(async context =>
                {

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature!=null)
                    {

                        ErrorDto dto = null;


                        if (errorFeature.Error is CustomException)
                        {

                            dto = new ErrorDto(errorFeature.Error.Message, true);

                        }

                        else
                        {
                            dto = new ErrorDto(errorFeature.Error.Message, false);
                        }



                        var response = Response<NoDataDto>.Fail(dto, 500);


                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));


                    }



                });
            });



        }
    }
}
