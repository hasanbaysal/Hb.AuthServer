using System.Text.Json.Serialization;

namespace Hb.AuthServer.Common.Dtos
{
    public class Response<T> where T:class
    {

        public T Data { get; private set; }
        public int StatusCoe { get; private set; }
        public ErrorDto Error { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; set; } //kendi iç sistemimizde response başarılı mı değil mi kontrol etmek için

        public static Response<T> Succes(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCoe = statusCode , IsSuccessful=true };
        }

        public static Response<T> Succes(int statusCode)
        {
            return new Response<T> {Data=default, StatusCoe = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(ErrorDto dto,int statusCode)
        {
            return new Response<T> { Error = dto,StatusCoe=statusCode, IsSuccessful = false };
        }
        public static Response<T> Fail(string errorMessage,int statusCode, bool isShow)
        {
            return new Response<T> { Error = new ErrorDto(errorMessage,isShow), StatusCoe=statusCode , IsSuccessful=false };
        }


    }



   

}
