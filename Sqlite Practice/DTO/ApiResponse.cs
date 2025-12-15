namespace Sqlite_Practice.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }      // true/false
        public string Message { get; set; }    // Human-readable message
        public T Data { get; set; }            // Optional payload
        public int StatusCode { get; set; }    // HTTP status code

        public ApiResponse(T data, string message = null, int statusCode = 200)
        {
            Success = statusCode >= 200 && statusCode < 300;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
    }
}
