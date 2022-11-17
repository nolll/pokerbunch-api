using System;
using System.Threading.Tasks;
using Core.Errors;

namespace Core.UseCases;

public abstract class UseCase<TRequest, TResult>
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
            return new UseCaseResult<TResult>(new UnknownError(e));
        }
    }

    protected UseCaseResult<TResult> Success(TResult result)
    {
        return new UseCaseResult<TResult>(result);
    }

    protected UseCaseResult<TResult> Error(UseCaseError error)
    {
        return new UseCaseResult<TResult>(error);
    }
}

public abstract class UseCase<TResult>
{
    protected abstract UseCaseResult<TResult> Work();

    public UseCaseResult<TResult> Execute()
    {
        try
        {
            return Work();
        }
        catch (Exception e)
        {
            return new UseCaseResult<TResult>(new UnknownError(e));
        }
    }

    protected UseCaseResult<TResult> Success(TResult result)
    {
        return new UseCaseResult<TResult>(result);
    }

    protected UseCaseResult<TResult> Error(UseCaseError error)
    {
        return new UseCaseResult<TResult>(error);
    }
}

public abstract class AsyncUseCase<TRequest, TResult>
{
    protected abstract Task<UseCaseResult<TResult>> Work(TRequest request);

    public async Task<UseCaseResult<TResult>> Execute(TRequest request)
    {
        try
        {
            return await Work(request);
        }
        catch (Exception e)
        {
            return new UseCaseResult<TResult>(new UnknownError(e));
        }
    }

    protected UseCaseResult<TResult> Success(TResult result)
    {
        return new UseCaseResult<TResult>(result);
    }

    protected UseCaseResult<TResult> Error(UseCaseError error)
    {
        return new UseCaseResult<TResult>(error);
    }
}

public abstract class AsyncUseCase<TResult>
{
    protected abstract Task<UseCaseResult<TResult>> Work();

    public async Task<UseCaseResult<TResult>> Execute()
    {
        try
        {
            return await Work();
        }
        catch (Exception e)
        {
            return new UseCaseResult<TResult>(new UnknownError(e));
        }
    }

    protected UseCaseResult<TResult> Success(TResult result)
    {
        return new UseCaseResult<TResult>(result);
    }

    protected UseCaseResult<TResult> Error(UseCaseError error)
    {
        return new UseCaseResult<TResult>(error);
    }
}
