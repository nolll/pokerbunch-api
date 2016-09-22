using Core.Entities;
using Core.Services;
using Infrastructure.Data.Factories;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Infrastructure.Data.Factories
{
    public class RawCashgameFactoryTests : TestBase
    {
        [Test]
        public void Create_WithoutStatus_StatusIsSetFromCashgame()
        {
            var sut = GetSut();
            var result = sut.Create(A.Cashgame.WithStatus(GameStatus.Finished).Build());

            Assert.AreEqual(result.Status, (int)GameStatus.Finished);
        }
        
        private RawCashgameFactory GetSut()
        {
            return new RawCashgameFactory(
                GetMock<ITimeProvider>().Object);
        }
    }
}
