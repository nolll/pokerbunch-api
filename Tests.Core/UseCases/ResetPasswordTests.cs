using Core;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;

namespace Tests.Core.UseCases;

public class ResetPasswordTests : TestBase
{
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    private readonly IRandomizer _randomizer = Substitute.For<IRandomizer>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    [Fact]
    public async Task ResetPassword_WithInvalidEmail_ValidationExceptionIsThrown()
    {
        var request = CreateRequest(email: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task ResetPassword_UserNotFound_ReturnsError()
    {
        var request = CreateRequest();
        var result = await Sut.Execute(request);
        
        result.Error!.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task ResetPassword_PasswordIsChangedAndEmailIsSent()
    {
        const string subject = "Poker Bunch Password Recovery";
        const string body = """
                            Here is your new password for Poker Bunch:
                            aaaaaaaa

                            Please sign in here: https://loginUrl
                            """;
        _randomizer.GetAllowedChars().Returns("a");

        var user = Create.User();
        _userRepository.GetByUserEmail(user.Email).Returns(user);
        var request = CreateRequest(email: user.Email, loginUrl: "https://loginUrl");
        await Sut.Execute(request);

        _emailSender.Received().Send(Arg.Is<string>(o => o == user.Email),
            Arg.Is<IMessage>(o => o.Subject == subject && o.Body == body));

        await _userRepository.Received().Update(Arg.Is<User>(o => o.Id == user.Id && 
                                                                  user.EncryptedPassword == "0478095c8ece0bbc11f94663ac2c4f10b29666de" &&
                                                                  user.Salt == "aaaaaaaaaa"));
    }

    private ResetPassword.Request CreateRequest(string? email = null, string? loginUrl = null)
    {
        return new ResetPassword.Request(
            email ?? Create.EmailAddress(),
            loginUrl ?? Create.String());
    }

    private ResetPassword Sut => new(
        _userRepository,
        _emailSender,
        _randomizer);
}