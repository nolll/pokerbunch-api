using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Moq;
using Tests.Common;

namespace Tests.Core.UseCases;

public class ChangePasswordTests : MockBase
{
    private Mock<IUserRepository> _userRepositoryMock = new();
    private Mock<IRandomizer> _randomizerMock = new();

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _randomizerMock = new Mock<IRandomizer>();
    }

    [Test]
    public async Task ChangePassword_EmptyPassword_ReturnsError()
    {
        var request = new ChangePassword.Request(TestData.UserNameA, "", "b");
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task ChangePassword_CurrentPasswordIsWrong_ReturnsError()
    {
        var user = new User("1", TestData.UserNameA, salt: "123456", encryptedPassword: "abcdef");
        _userRepositoryMock.Setup(o => o.GetByUserName(TestData.UserNameA)).Returns(Task.FromResult(user));

        var request = new ChangePassword.Request(TestData.UserNameA, "new-password", "current-password");
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Auth));
    }

    [Test]
    public async Task ChangePassword_EqualPasswords_SavesUserWithNewPassword()
    {
        var user = new User("1", TestData.UserNameA, salt: "123456", encryptedPassword: "9217510d5221554de3230b5634a3f81e3cf19d94");
        _userRepositoryMock.Setup(o => o.GetByUserName(TestData.UserNameA)).Returns(Task.FromResult(user));

        User? savedUser = null;
        _userRepositoryMock.Setup(o => o.Update(It.IsAny<User>())).Callback((User u) => savedUser = u);

        _randomizerMock.Setup(o => o.GetAllowedChars()).Returns("a");
            
        var request = new ChangePassword.Request(TestData.UserNameA, "new-password", "current-password");
        await Sut.Execute(request);
            
        Assert.That(savedUser?.EncryptedPassword, Is.EqualTo("cebb55b2a2b59b692bf5c81c9359b59c3244fe86"));
    }
        
    private ChangePassword Sut => new(
        _userRepositoryMock.Object,
        _randomizerMock.Object);
}