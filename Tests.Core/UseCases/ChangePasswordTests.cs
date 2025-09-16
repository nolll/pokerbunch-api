using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;

namespace Tests.Core.UseCases;

public class ChangePasswordTests : TestBase
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IRandomizer _randomizer = Substitute.For<IRandomizer>();

    [Fact]
    public async Task ChangePassword_EmptyPassword_ReturnsError()
    {
        var user = Create.User(salt: "123456", encryptedPassword: "abcdef");
        var request = new ChangePassword.Request(user.UserName, "", "b");
        _userRepository.GetByUserName(user.UserName).Returns(Task.FromResult(user));
        
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task ChangePassword_CurrentPasswordIsWrong_ReturnsError()
    {
        var user = Create.User(salt: "123456", encryptedPassword: "abcdef");
        _userRepository.GetByUserName(user.UserName).Returns(Task.FromResult(user));

        var request = new ChangePassword.Request(user.UserName, "new-password", "current-password");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Auth);
    }

    [Fact]
    public async Task ChangePassword_EqualPasswords_SavesUserWithNewPassword()
    {
        var user = Create.User(salt: "123456", encryptedPassword: "9217510d5221554de3230b5634a3f81e3cf19d94");
        _userRepository.GetByUserName(user.UserName).Returns(Task.FromResult(user));
        _randomizer.GetAllowedChars().Returns("a");
            
        var request = new ChangePassword.Request(user.UserName, "new-password", "current-password");
        await Sut.Execute(request);
        
        await _userRepository.Received().Update(Arg.Is<User>(o => o.EncryptedPassword == "cebb55b2a2b59b692bf5c81c9359b59c3244fe86"));
    }
        
    private ChangePassword Sut => new(_userRepository, _randomizer);
}