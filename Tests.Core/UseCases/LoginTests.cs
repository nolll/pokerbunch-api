using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class LoginTests : TestBase
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();

    [Test]
    public async Task Login_UserNotFound_ReturnsError()
    {
        var user = Create.User();
        _userRepository.GetByUserNameOrEmail(user.UserName).Returns(user);
        
        var request = CreateRequest();
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    [Test]
    public async Task Login_UserFoundButPasswordIsWrong_ReturnsError()
    {
        var user = Create.User(salt: "aaaaaaaaaa", encryptedPassword: "1cb313748ba4b822b78fe05de42558539efd9156");
        _userRepository.GetByUserNameOrEmail(user.UserName).Returns(user);
        
        var request = CreateRequest(user.UserName, "a");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    [Test]
    public async Task Login_UserFoundAndPasswordIsCorrect_UserIsLoggedIn()
    {
        var user = Create.User(salt: "aaaaaaaaaa", encryptedPassword: "1cb313748ba4b822b78fe05de42558539efd9156");
        _userRepository.GetByUserNameOrEmail(user.UserName).Returns(user);

        var bunch = Create.Bunch();
        _bunchRepository.List(user.Id).Returns([bunch]);

        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.Get(bunch.Id, user.Id).Returns(player);   
        
        var request = CreateRequest(user.UserName, "c");
        var result = await Sut.Execute(request);
        
        result.Data!.UserName.Should().Be(user.UserName);
        result.Data.BunchResults.Count.Should().Be(1);

        var userBunch = result.Data.BunchResults.First();
        userBunch.BunchId.Should().Be(bunch.Id);
        userBunch.BunchName.Should().Be(bunch.DisplayName);
        userBunch.BunchSlug.Should().Be(bunch.Slug);
        userBunch.PlayerId.Should().Be(player.Id);
        userBunch.PlayerName.Should().Be(player.DisplayName);
        userBunch.Role.Should().Be(player.Role);
    }
      
    private Login.Request CreateRequest(string? userName = null, string? password = null) => new(
        userName ?? Create.String(), 
        password ?? Create.String());

    private Login Sut => new(_userRepository, _bunchRepository, _playerRepository);
}