namespace Application.Core;

public class ResponseResult<T>
{
    public bool IsSuccess { get; set; }
    public T Value { get; set; }
    public string Error { get; set; }

    public static ResponseResult<T> Success(T value) => new ResponseResult<T> { IsSuccess = true, Value = value };
    public static ResponseResult<T> Failure(string error) => new ResponseResult<T> { IsSuccess = false, Error = error };
}