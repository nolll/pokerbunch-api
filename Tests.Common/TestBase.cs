using System.Net.Mail;
using AutoFixture;
using Core.Entities;
using NUnit.Framework;

namespace Tests.Common;

public class TestBase
{
    protected TestDependencies Deps { get; private set; }
    protected readonly Fixture Fixture = new();

    public TestBase()
    {
        Deps = new TestDependencies();
    }

    [SetUp]
    public void ClearFakes()
    {
        Deps = new TestDependencies();
    }

    protected string CreateEmailAddress() => Fixture.Create<MailAddress>().Address; 
    
    protected User CreateUser(
        string? id = null,
        string? userName = null,
        string? displayName = null,
        string? realName = null,
        string? email = null,
        Role globalRole = Role.Player,
        string? encryptedPassword = null,
        string? salt = null)
    {
        return new User(
            id ?? Fixture.Create<string>(), 
            userName ?? Fixture.Create<string>(), 
            displayName ?? Fixture.Create<string>(),
            realName ?? Fixture.Create<string>(),
            email ?? CreateEmailAddress(), 
            globalRole, 
            encryptedPassword, 
            salt);
    }
}