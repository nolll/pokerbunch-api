using System;
using System.Net.Mail;
using AutoFixture;
using Core.Entities;
using Core.Entities.Checkpoints;

namespace Tests.Common;

public class TestDataFactory
{
    private readonly Fixture _fixture = new();
    
    private T Create<T>() => _fixture.Create<T>();
    public T Type<T>() => Create<T>();
    public string String() => Create<string>();
    public int Int() => Create<int>();
    public DateTime DateTime() => Create<DateTime>();
    public string EmailAddress() => Create<MailAddress>().Address;
    public string TimeZoneId() => Create<TimeZoneInfo>().Id;
    public Date Date() => new(Create<DateTime>());

    public User User(
        string? id = null,
        string? userName = null,
        string? displayName = "default",
        string? realName = null,
        string? email = null,
        Role globalRole = Role.Player,
        string? encryptedPassword = null,
        string? salt = null) => new(
        id ?? String(),
        userName ?? String(),
        displayName ?? String(),
        realName ?? String(),
        email ?? EmailAddress(),
        globalRole,
        encryptedPassword,
        salt);

    public Bunch Bunch(
        string? id = null,
        string? slug = null,
        string? displayName = null,
        string? description = null,
        string? houseRules = null,
        TimeZoneInfo? timezone = null,
        int? defaultBuyin = null,
        Currency? currency = null) => new(
        id ?? String(),
        slug ?? String(),
        displayName ?? String(),
        description ?? String(),
        houseRules ?? String(),
        timezone ?? Create<TimeZoneInfo>(),
        defaultBuyin ?? Int(),
        currency ?? Create<Currency>());

    public Location Location(string? id = null, string? name = null, string? bunchId = null) => new(
        id ?? String(),
        name ?? String(),
        bunchId ?? String());
    
    public Event Event(
        string? id = null,
        string? name = null,
        string? bunchId = null,
        string? locationId = null,
        Date? startDate = null,
        Date? endDate = null) =>
        new(
        id ?? String(),
        name ?? String(),
        bunchId ?? String(),
        locationId ?? String(),
        startDate ?? Date(),
        endDate ?? Date());

    public Player Player(
        string? bunchId = null,
        string? id = null,
        string? userId = null,
        string? userName = null,
        string? displayName = null,
        Role? role = null,
        string? color = null) => new(
        bunchId ?? String(),
        id ?? String(),
        userId,
        userName,
        displayName ?? String(),
        role ?? Role.Player,
        color);

    public UserBunch UserBunch(
        string? id = null,
        string? slug = null,
        string? name = null,
        string? playerId = null,
        string? playerName = null,
        Role? role = null) => new(
        id ?? String(),
        slug ?? String(),
        name ?? String(),
        playerId ?? String(),
        playerName ?? String(),
        role ?? Type<Role>());
    
    public UserBunch UserBunch(Bunch bunch, Player player) 
        => UserBunch(bunch.Id, bunch.Slug, bunch.DisplayName, player.Id, player.DisplayName, player.Role);
    
    public UserBunch UserBunch(Bunch bunch) 
        => UserBunch(bunch.Id, bunch.Slug, bunch.DisplayName);

    public Cashgame Cashgame(
        string? bunchId = null,
        string? locationId = null,
        string? eventId = null,
        GameStatus? status = null,
        string? id = null) => new(
        bunchId ?? String(),
        locationId ?? String(),
        eventId,
        status ?? Create<GameStatus>(),
        id ?? String(),
        []);

    public Checkpoint BuyinAction(
        string? id = null,
        string? cashgameId = null,
        string? playerId = null,
        DateTime? timestamp = null,
        int? stack = null,
        int? buyin = null) => Checkpoint.Create(
        id ?? String(),
        cashgameId ?? String(),
        playerId ?? String(),
        timestamp ?? DateTime(),
        CheckpointType.Buyin,
        stack ?? Int(),
        buyin ?? Int());

    public Checkpoint ReportAction(
        string? id = null,
        string? cashgameId = null,
        string? playerId = null,
        DateTime? timestamp = null,
        int? stack = null) => Checkpoint.Create(
        id ?? String(),
        cashgameId ?? String(),
        playerId ?? String(),
        timestamp ?? DateTime(),
        CheckpointType.Report,
        stack ?? Int());

    public Checkpoint CashoutAction(
        string? id = null,
        string? cashgameId = null,
        string? playerId = null,
        DateTime? timestamp = null,
        int? stack = null) => Checkpoint.Create(
        id ?? String(),
        cashgameId ?? String(),
        playerId ?? String(),
        timestamp ?? DateTime(),
        CheckpointType.Cashout,
        stack ?? Int());
}