using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Moq;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases.CashoutTests
{
    public abstract class Arrange : ArrangeBase
    {
        private const int BunchId = 1;
        private const int CashgameId = 2;
        private const int LocationId = 3;
        private const int UserId = 4;
        protected const int PlayerId = 5;
        protected const string Slug = "slug";
        protected const string UserName = "username";
        private DateTime _startTime = DateTime.Parse("2001-01-01 12:00:00");

        protected virtual int CashoutStack => 123;
        protected DateTime CashoutTime => _startTime.AddMinutes(1);
        protected virtual bool HasCashedOutBefore => false;

        protected int CheckpointCountBeforeCashout;
        protected Cashgame UpdatedCashgame;
        protected Cashout Sut;

        [SetUp]
        public void Setup()
        {
            Sut = CreateSut<Cashout>();

            var cashgame = CreateCashgame();
            CheckpointCountBeforeCashout = cashgame.Checkpoints.Count;
            MockOf<IBunchService>().Setup(s => s.GetBySlug(Slug)).Returns(new Bunch(BunchId, Slug));
            MockOf<ICashgameService>().Setup(s => s.GetRunning(BunchId)).Returns(CreateCashgame());
            MockOf<IPlayerService>().Setup(s => s.GetByUserId(BunchId, UserId)).Returns(new Player(BunchId, PlayerId, UserId));
            MockOf<IUserRepository>().Setup(s => s.Get(UserName)).Returns(new User(UserId, UserName));

            MockOf<ICashgameService>().Setup(o => o.UpdateGame(It.IsAny<Cashgame>())).Callback((Cashgame c) => UpdatedCashgame = c);
        }

        private Cashgame CreateCashgame()
        {
            if (HasCashedOutBefore)
            {
                var checkpoints1 = new List<Checkpoint>
                {
                    Checkpoint.Create(CashgameId, PlayerId, _startTime, CheckpointType.Buyin, 200, 200, 1),
                    Checkpoint.Create(CashgameId, PlayerId, _startTime.AddMinutes(1), CheckpointType.Cashout, 200, 0, 3)
                };

                return new Cashgame(BunchId, LocationId, GameStatus.Running, CashgameId, checkpoints1);
            }
            else
            {
                var checkpoints1 = new List<Checkpoint>
                {
                    Checkpoint.Create(CashgameId, PlayerId, _startTime, CheckpointType.Buyin, 200, 200, 1)
                };

                return new Cashgame(BunchId, LocationId, GameStatus.Running, CashgameId, checkpoints1);
            }
        }
    }
}