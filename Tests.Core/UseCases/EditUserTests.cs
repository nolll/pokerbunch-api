using AutoFixture;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditUserTests : TestBase
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    [Test]
    public async Task EditUser_EmptyDisplayName_ReturnsError()
    {
        var request = CreateRequest(displayName: "");
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_EmptyEmail_ReturnsError()
    {
        var request = CreateRequest(email: "");
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_InvalidEmail_ReturnsError()
    {
        var request = CreateRequest(email: "a");
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_ValidInput_UserNameIsSet()
    {
        var userName = Fixture.Create<string>();
        var request = CreateRequest(userName: userName);
        var result = await Sut.Execute(request);

        result.Data?.UserName.Should().Be(userName);
    }

    [Test]
    public async Task EditUser_ValidInput_UserIsSaved()
    {
        var userName = Fixture.Create<string>();
        var displayName = Fixture.Create<string>();
        const string email = "test@example.com";
        var user = new User("", userName, displayName, null, email);
        
        var changedDisplayName = Fixture.Create<string>();
        const string changedEmail = "changed@example.com";

        _userRepository.GetByUserName(userName).Returns(user);
        
        var request = CreateRequest(userName, changedDisplayName, email: changedEmail);

        await Sut.Execute(request);

        await _userRepository.Received()
            .Update(Arg.Is<User>(o => o.UserName == userName && o.DisplayName == changedDisplayName && o.Email == changedEmail));
    }

    private EditUser.Request CreateRequest(
        string? userName = null,
        string? displayName = null,
        string? realName = null,
        string? email = null)
    {
        return new EditUser.Request(
            userName ?? Fixture.Create<string>(),
            displayName ?? Fixture.Create<string>(),
            realName ?? Fixture.Create<string>(),
            email ?? "test@example.com");
    }

    private EditUser Sut => new(_userRepository);
}