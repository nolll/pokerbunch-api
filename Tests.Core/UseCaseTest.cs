using Moq;

namespace Tests.Core;

public abstract class UseCaseTest<T> where T : class
{
    private Mocker _mocker;
    protected T Sut { get; private set; }

    [SetUp]
    public void UseCaseSetup()
    {
        _mocker = new Mocker();
        Sut = _mocker.New<T>();
        Setup();
        Execute();
        ExecuteAsync();
    }

    protected Mock<TM> Mock<TM>() where TM : class => _mocker.MockOf<TM>();

    protected abstract void Setup();

    protected virtual void Execute()
    {

    }

    protected virtual Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}