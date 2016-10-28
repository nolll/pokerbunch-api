﻿using Tests.Common.FakeRepositories;
using Tests.Common.FakeServices;

namespace Tests.Common
{
    public class TestDependencies
    {
        public FakeBunchRepository Bunch { get; }
        public FakeUserRepository User { get; }
        public FakePlayerRepository Player { get; }
        public FakeCashgameRepository Cashgame { get; }
        public FakeEventRepository Event { get; }
        public FakeLocationRepository Location { get; }
        public FakeAppRepository App { get; }

        public FakeMessageSender MessageSender { get; }
        public FakeRandomizer Randomizer { get; }

        public TestDependencies()
        {
            Bunch = new FakeBunchRepository();
            User = new FakeUserRepository();
            Player = new FakePlayerRepository();
            Cashgame = new FakeCashgameRepository();
            Event = new FakeEventRepository();
            Location = new FakeLocationRepository();
            App = new FakeAppRepository();

            MessageSender = new FakeMessageSender();
            Randomizer = new FakeRandomizer();
        }

        public void Clear()
        {
            MessageSender.Reset();
        }
    }
}