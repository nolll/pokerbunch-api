﻿using Core.Services;
using Tests.Common.FakeServices;

namespace Tests.Common
{
    public class ServiceContainer
    {
        public FakeMessageSender MessageSender { get; }
        public FakeRandomService RandomService { get; }
        public BunchService BunchService { get; }
        public CashgameService CashgameService { get; }
        public UserService UserService { get; }
        public EventService EventService { get; }
        public PlayerService PlayerService { get; }
        public LocationService LocationService { get; }
        public AppService AppService { get; }

        public ServiceContainer(RepositoryContainer repos)
        {
            MessageSender = new FakeMessageSender();
            RandomService = new FakeRandomService();
            BunchService = new BunchService(repos.Bunch);
            CashgameService = new CashgameService(repos.Cashgame);
            UserService = new UserService(repos.User);
            EventService = new EventService(repos.Event);
            PlayerService = new PlayerService(repos.Player);
            LocationService = new LocationService(repos.Location);
            AppService = new AppService(repos.App);
        }

        public void Clear()
        {
            MessageSender.Reset();
        }
    }
}