using Core;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddUserTests : TestBase
{
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    private readonly IRandomizer _randomizer = Substitute.For<IRandomizer>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    [Fact]
    public async Task AddUser_WithEmptyUserName_ReturnsError()
    {
        var request = CreateRequest(userName: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddUser_WithEmptyEmail_ReturnsError()
    {
        var request = CreateRequest(email: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddUser_WithEmptyPassword_ReturnsError()
    {
        var request = CreateRequest(password: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddUser_UserNameAlreadyInUse_ReturnsError()
    {
        var existingUser = Create.User();
        _userRepository.GetByUserName(existingUser.UserName).Returns(existingUser);
        
        var request = CreateRequest(userName: existingUser.UserName);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task AddUser_EmailAlreadyInUse_ReturnsError()
    {
        var existingUser = Create.User();
        _userRepository.GetByUserEmail(existingUser.Email).Returns(existingUser);
        
        var request = CreateRequest(email: existingUser.Email);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task AddUser_WithValidInput_UserWithCorrectPropertiesIsAdded()
    {
        const string password = "c";
        const string expectedEncryptedPassword = "1cb313748ba4b822b78fe05de42558539efd9156";
        const string expectedSalt = "aaaaaaaaaa";

        var userName = Create.String();
        var displayName = Create.String();
        var email = Create.EmailAddress();

        _randomizer.GetAllowedChars().Returns("a");
        
        var request = CreateRequest(userName, displayName, email, password);
        await Sut.Execute(request);

        await _userRepository.Received().Add(Arg.Is<User>(o => 
            o.UserName == userName && 
            o.DisplayName == displayName &&
            o.RealName == "" &&
            o.Email == email &&
            o.GlobalRole == Role.Player &&
            o.EncryptedPassword == expectedEncryptedPassword &&
            o.Salt == expectedSalt));
    }
    
    [Fact]
    public async Task AddUser_WithoutDisplayName_UserNameIsUsedAsDisplayName()
    {
        var userName = Create.String();

        _randomizer.GetAllowedChars().Returns("a");
        
        var request = CreateRequest(userName, "");
        await Sut.Execute(request);

        await _userRepository.Received().Add(Arg.Is<User>(o => 
            o.UserName == userName && 
            o.DisplayName == userName));
    }

    [Fact]
    public async Task AddUser_WithValidInput_SendsRegistrationEmail()
    {
        const string subject = "Poker Bunch Registration";
        const string body = """
                            Thanks for registering with Poker Bunch.

                            Please sign in here: /loginUrl
                            """;

        var request = CreateRequest(loginUrl: "/loginUrl");
        await Sut.Execute(request);

        _emailSender.Received().Send(Arg.Is<string>(o => o == request.Email),
            Arg.Is<IMessage>(o => o.Subject == subject && o.Body == body));
    }
    
    private AddUser.Request CreateRequest(
        string? userName = null,
        string? displayName = null,
        string? email = null,
        string? password = null,
        string? loginUrl = null)
    {
        return new AddUser.Request(
            userName ?? Create.String(), 
            displayName ?? Create.String(), 
            email ?? Create.EmailAddress(), 
            password ?? Create.String(), 
            loginUrl ?? "/");
    }

    private AddUser Sut => new(
        _userRepository,
        _randomizer,
        _emailSender);
}