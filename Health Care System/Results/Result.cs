namespace Health_Care_System.Results;

/// <summary>
/// Outcome of a service operation that has no payload.
/// Services return this instead of throwing, so the controller decides the status code.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string Error { get; }

    public Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string error) => new(false, error);
}
