using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetBunchList
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;

    public GetBunchList(IBunchRepository bunchRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
    }

    public Result Execute(AllBunchesRequest request)
    {
        var user = _userRepository.Get(request.UserName);
        RequireRole.Admin(user);

        var bunches = _bunchRepository.List();
        return new Result(bunches);
    }

    public Result Execute(UserBunchesRequest request)
    {
        var user = _userRepository.Get(request.UserName);
        var bunches = user != null ? _bunchRepository.List(user.Id) : new List<Bunch>();
            
        return new Result(bunches);
    }

    public class AllBunchesRequest
    {
        public string UserName { get; }

        public AllBunchesRequest(string userName)
        {
            UserName = userName;
        }
    }

    public class UserBunchesRequest
    {
        public string UserName { get; }

        public UserBunchesRequest(string userName)
        {
            UserName = userName;
        }
    }

    public class Result
    {
        public IList<ResultItem> Bunches { get; private set; }

        public Result(IEnumerable<Bunch> bunches)
        {
            Bunches = bunches.Select(o => new ResultItem(o)).ToList();
        }
    }

    public class ResultItem
    {
        public string Slug { get; }
        public string Name { get; }
        public string Description { get; }

        public ResultItem(Bunch bunch)
        {
            Slug = bunch.Slug;
            Name = bunch.DisplayName;
            Description = bunch.Description;
        }
    }
}