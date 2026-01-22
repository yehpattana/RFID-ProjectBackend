namespace RFIDApi.DTO
{
    public class ResponseDTO<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusCode { get; set; }
    }

    public class ResponseDTO
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusCode { get; set; }
    }
}
