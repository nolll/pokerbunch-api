using System;

namespace Core.UseCases;

public abstract class UseCase<TResult, TRequest>
{
    protected abstract UseCaseResult<TResult> Work(TRequest request);

    public UseCaseResult<TResult> Execute(TRequest request)
    {
        try
        {
            return Work(request);
        }
        catch (Exception e)
        {
            return new UseCaseResult<TResult>(new UseCaseError(e));
        }
    }
}