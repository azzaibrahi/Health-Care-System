namespace Health_Care_System.Results;

/// <summary>
/// Outcome of a service operation that carries a payload on success
/// and an error message on failure.
/// </summary>
/// <typeparam name="T">Type of the payload.</typeparam>
public class GenericResult<T> : Result
{
    public T? Value { get; set; }

    public GenericResult(bool isSuccess, T? value, string error)
        : base(isSuccess, error)
        => Value = value;

    public static GenericResult<T> Success(T value)
        => new(true, value, string.Empty);

    // Hides Result.Failure on purpose: this overload returns a payload-carrying failure.
    public static new GenericResult<T> Failure(string error)
        => new(false, default, error);
}
