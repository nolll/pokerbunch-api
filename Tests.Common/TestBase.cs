using System;
using System.Net.Mail;
using AutoFixture;
using Core.Entities;
using Core.Entities.Checkpoints;
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
    
    protected Bunch CreateBunch(
        string? id = null,
        string? slug = null,
        string? displayName = null,
        string? description = null,
        string? houseRules = null,
        TimeZoneInfo? timezone = null,
        int? defaultBuyin = null,
        Currency? currency = null)
    {
        return new Bunch(
            id ?? Fixture.Create<string>(),
            slug ?? Fixture.Create<string>(),
            displayName ?? Fixture.Create<string>(),
            description ?? Fixture.Create<string>(),
            houseRules ?? Fixture.Create<string>(),
            timezone ?? Fixture.Create<TimeZoneInfo>(),
            defaultBuyin ?? Fixture.Create<int>(),
            currency ?? Fixture.Create<Currency>());
    }

    protected Cashgame CreateCashgame(
        string? bunchId = null,
        string? locationId = null,
        string? eventId = null,
        GameStatus? status = null,
        string? id = null,
        IList<Checkpoint>? checkpoints = null)
    {
        return new Cashgame(
            bunchId ?? Fixture.Create<string>(),
            locationId ?? Fixture.Create<string>(),
            eventId ?? Fixture.Create<string>(),
            status ?? Fixture.Create<GameStatus>(),
            id ?? Fixture.Create<string>(),
            checkpoints ?? []);
    }
}