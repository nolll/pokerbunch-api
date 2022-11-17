﻿using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;

namespace Core.UseCases;

public class GetBunchListForUser : UseCase<GetBunchListForUser.Request, GetBunchListForUser.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;

    public GetBunchListForUser(IBunchRepository bunchRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        var bunches = user != null ? _bunchRepository.List(user.Id) : new List<Bunch>();

        return Success(new Result(bunches));
    }
    
    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }

    public class Result : BunchListResult
    {
        public Result(IEnumerable<Bunch> bunches) : base(bunches)
        {
        }
    }
}