using Microsoft.AspNetCore.Http.HttpResults;
using RFIDApi.DTO;

namespace RFIDApi.Helper
{
    public static class ResponseFactory<T> where T : class
    {

        public static ResponseDTO<T> Ok(string message,T Data = default)
        {
            return new ResponseDTO<T>
            {
                Data = Data,
                IsSuccess = true,
                Message = message,
                StatusCode = "200"
            };
        }
        

        public static ResponseDTO<T> Failed(string message)
        {
            return new ResponseDTO<T>
            {
                Data = default,
                IsSuccess = false,
                Message = message,
                StatusCode = "500"
            };
        }
    }
}
