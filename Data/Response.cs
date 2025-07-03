
namespace Data
{
    public class ApiResponse<R>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public R? Data { get; set; }

        public static ApiResponse<R> SuccessResponse(R? data, string? message = null, int statusCode = 200)
        {
            return new ApiResponse<R>
            {
                StatusCode = statusCode,
                Success = true,
                Message = message,
                Data = data
                
            };

        }
        public static ApiResponse<R> ErrorResponse(string? message = null,int statusCode = 400)
        {
             return new ApiResponse<R>
            {
                StatusCode = statusCode,
                Success=false,
                Message = message,
                
            };

        }
    }
}