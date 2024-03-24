using Core.Entities;
using Core.Repositories;

namespace Core.UseCases;

public class GetBunchListForUser(IBunchRepository bunchRepository, IUserRepository userRepository)
    : UseCase<GetBunchListForUser.Request, GetBunchListForUser.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await userRepository.GetByUserName(request.UserName);
        var bunches = await bunchRepository.List(user.Id);

        return Success(new Result(bunches));
    }
    
    public class Request(string userName)
    {
        public string UserName { get; } = userName;
    }

    public class Result(IEnumerable<Bunch> bunches) : BunchListResult(bunches);
}