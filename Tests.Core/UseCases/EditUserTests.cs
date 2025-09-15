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

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_EmptyEmail_ReturnsError()
    {
        var request = CreateRequest(email: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_InvalidEmail_ReturnsError()
    {
        var request = CreateRequest(email: "a");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_ValidInput_UserIsSaved()
    {
        var user = CreateUser();
        
        var changedDisplayName = Fixture.Create<string>();
        var changedEmail = CreateEmailAddress();

        _userRepository.GetByUserName(user.UserName).Returns(user);
        
        var request = CreateRequest(user.UserName, changedDisplayName, email: changedEmail);

        await Sut.Execute(request);

        await _userRepository.Received()
            .Update(Arg.Is<User>(o => o.UserName == user.UserName && o.DisplayName == changedDisplayName && o.Email == changedEmail));
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
            email ?? CreateEmailAddress());
    }

    private EditUser Sut => new(_userRepository);
}