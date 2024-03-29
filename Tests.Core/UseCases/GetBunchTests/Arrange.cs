﻿using Core.Entities;
using Core.Repositories;
using Core.UseCases;

namespace Tests.Core.UseCases.GetBunchTests;

public abstract class Arrange : UseCaseTest<GetBunch>
{
    protected UseCaseResult<GetBunch.Result>? Result;

    private const string BunchId = "1";
    private const string UserId = "4";
    private const string PlayerId = "5";
    private const string Slug = "slug";
    private const string UserName = "username";
    private const string PlayerName = "playername";
    protected const string DisplayName = "displayname";
    protected const string Description = "description";
    protected const string HouseRules = "houserules";
    protected virtual Role Role => Role.None;
        
    protected override void Setup()
    {
        var bunch = new Bunch(BunchId, Slug, DisplayName, Description, HouseRules);
        var player = new Player(BunchId, PlayerId, UserId, UserName, PlayerName, Role);
        var user = new User(UserId, UserName);

        Mock<IBunchRepository>().Setup(s => s.GetBySlug(Slug)).Returns(Task.FromResult(bunch));
        Mock<IPlayerRepository>().Setup(s => s.Get(BunchId, UserId)).Returns(Task.FromResult<Player?>(player));
        Mock<IUserRepository>().Setup(s => s.GetByUserName(UserName)).Returns(Task.FromResult(user));
    }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new GetBunch.Request(UserName, Slug));
    }
}