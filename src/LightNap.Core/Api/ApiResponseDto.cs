namespace LightNap.Core.Api
{
    public class ApiResponseDto<T>
    {
        public T? Result { get; set; }
        public ApiResponseType Type { get; set; }
        public IEnumerable<string>? ErrorMessages { get; set; }

        public ApiResponseDto() { }

        public ApiResponseDto(T? result)
        {
            Result = result;
            Type = ApiResponseType.Success;
        }

        public static ApiResponseDto<T> Success(T result) =>
            new(result) { Type = ApiResponseType.Success };

        public static ApiResponseDto<T> Fail(params string[] errors) =>
            new()
            {
                Type = ApiResponseType.Error,
                ErrorMessages = errors
            };
    }
}
