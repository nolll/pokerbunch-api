namespace Core.UseCases;

public class UseCaseResult<T>
{
    public UseCaseError Error { get; }
    public T Data { get; }
    public bool Success => Error == null;

    public UseCaseResult(UseCaseError error)
    {
        Error = error;
    }

    public UseCaseResult(T data)
    {
        Data = data;
    }
}