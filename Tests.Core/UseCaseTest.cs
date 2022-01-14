using Moq;
using NUnit.Framework;

namespace Tests.Core;

public abstract class UseCaseTest<T> where T : class
{
    private Mocker _mocker;
    protected T Sut { get; private set; }
    protected virtual bool ExecuteAutomatically => true;

    [SetUp]
    public void UseCaseSetup()
    {
        _mocker = new Mocker();
        Sut = _mocker.New<T>();
        Setup();
        if (ExecuteAutomatically) Execute();
    }

    protected Mock<TM> Mock<TM>() where TM : class => _mocker.MockOf<TM>();

    protected abstract void Setup();
    protected abstract void Execute();
}