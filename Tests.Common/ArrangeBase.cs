using Moq;
using NUnit.Framework;

namespace Tests.Common
{
    public class ArrangeBase
    {
        private Mocker _mocker;

        [SetUp]
        public void CreateMocker()
        {
            _mocker = new Mocker();
        }

        protected T CreateSut<T>()
        {
            return _mocker.New<T>();
        }

        protected Mock<T> MockOf<T>() where T : class
        {
            return _mocker.MockOf<T>();
        }
    }
}