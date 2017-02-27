using Moq;
using NUnit.Framework;

namespace Tests.Core
{
    public abstract class UseCaseTest<T> where T : class
    {
        private DependencyMocker _dependencyMocker;
        protected T Sut { get; private set; }
        protected virtual bool ExecuteAutomatically => true;

        [SetUp]
        public void UseCaseSetup()
        {
            _dependencyMocker = new DependencyMocker();
            Sut = _dependencyMocker.New<T>();
            Setup();
            if (ExecuteAutomatically) Execute();
        }

        protected Mock<TM> Mock<TM>() where TM : class => _dependencyMocker.MockOf<TM>();

        protected abstract void Setup();
        protected abstract void Execute();
    }
}
