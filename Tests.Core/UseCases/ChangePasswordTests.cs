using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Moq;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class ChangePasswordTests : MockBase
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IRandomizer> _randomizerMock;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _randomizerMock = new Mock<IRandomizer>();
    }

    [Test]
    public void ChangePassword_EmptyPassword_ThrowsValidationException()
    {
        var request = new ChangePassword.Request(TestData.UserNameA, "", "b");

        Assert.Throws<ValidationException>(() => Sut.Execute(request));
    }

    [Test]
    public void ChangePassword_CurrentPasswordIsWrong_ThrowsAuthException()
    {
        var user = new User(1, TestData.UserNameA, salt: "123456", encryptedPassword: "abcdef");
        _userRepositoryMock.Setup(o => o.Get(TestData.UserNameA)).Returns(user);

        var request = new ChangePassword.Request(TestData.UserNameA, "new-password", "current-password");

        Assert.Throws<AuthException>(() => Sut.Execute(request));
    }

    [Test]
    public void ChangePassword_EqualPasswords_SavesUserWithNewPassword()
    {
        var user = new User(1, TestData.UserNameA, salt: "123456", encryptedPassword: "9217510d5221554de3230b5634a3f81e3cf19d94");
        _userRepositoryMock.Setup(o => o.Get(TestData.UserNameA)).Returns(user);

        User savedUser = null;
        _userRepositoryMock.Setup(o => o.Update(It.IsAny<User>())).Callback((User u) => savedUser = u);

        _randomizerMock.Setup(o => o.GetAllowedChars()).Returns("a");
            
        var request = new ChangePassword.Request(TestData.UserNameA, "new-password", "current-password");
        Sut.Execute(request);
            
        Assert.That(savedUser.EncryptedPassword, Is.EqualTo("cebb55b2a2b59b692bf5c81c9359b59c3244fe86"));
    }
        
    private ChangePassword Sut => new ChangePassword(
        _userRepositoryMock.Object,
        _randomizerMock.Object);
}