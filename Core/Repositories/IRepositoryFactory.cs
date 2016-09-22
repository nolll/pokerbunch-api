namespace Core.Repositories
{
    public interface IRepositoryFactory
    {
        IAppRepository CreateAppRepository(IAppRepository appRepository);
        IBunchRepository CreateBunchRepository(IBunchRepository bunchRepository);
        ICashgameRepository CreateCashgameRepository(ICashgameRepository cashgameService);
        IEventRepository CreateEventRepository(IEventRepository eventRepository);
        IPlayerRepository CreatePlayerRepository(IPlayerRepository playerRepository);
        IUserRepository CreateUserRepository(IUserRepository userRepository);
    }
}