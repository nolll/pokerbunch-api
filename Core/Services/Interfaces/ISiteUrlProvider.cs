namespace Core.Services.Interfaces;

public interface ISiteUrlProvider
{
    string Login();
    string JoinRequestList(string bunchId);
}