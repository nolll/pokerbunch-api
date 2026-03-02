namespace Core.Services.Interfaces;

public interface IApiUrlProvider
{
    string Cashgame(string cashgameId);
    string User(string userName);
}