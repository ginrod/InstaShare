namespace InstaShare.Application.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int? TotalRecords { get; set; }

        public ApiResponse(bool success, string message, T data, int? totalRecords = null)
        {
            Success = success;
            Message = message;
            Data = data;
            TotalRecords = totalRecords;
        }
    }
}
