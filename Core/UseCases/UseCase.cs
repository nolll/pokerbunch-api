﻿using System;
using Core.Errors;

namespace Core.UseCases;

public abstract class UseCase<TRequest, TResult>
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
