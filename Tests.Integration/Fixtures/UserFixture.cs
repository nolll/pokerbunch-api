using Api.Models.UserModels;

namespace Tests.Integration.Fixtures;

public class UserFixture(ApiClientForTest apiClient, AddUserPostModel parameters, string token, string refreshToken)
{
    public string UserName { get; } = parameters.UserName;
    public string Password { get; } = parameters.Password;
    public string Email { get; } = parameters.Email;
    public string Token { get; private set; } = token;
    public string RefreshToken { get; } = refreshToken;
    
    public async Task Refresh()
    {
        var result = await apiClient.Auth.Refresh(new(RefreshToken));
        
        if (result.Success)
            Token = result.Model!.AccessToken;
        else
            throw new Exception("Refresh failed");
    }
}