using Api.Models.UserModels;
using Core.Entities;
using Infrastructure.Sql.Models;

namespace Tests.Integration.Fixtures;

public class UserFixture(PokerBunchDbContext db, ApiClientForTest apiClient, AddUserPostModel parameters, string token, string refreshToken)
{
    public string UserName { get; } = parameters.UserName;
    public string Password { get; } = parameters.Password;
    public string Email { get; } = parameters.Email;
    public string DisplayName { get; } = parameters.DisplayName;
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

    public async Task<UserFixture> AsAdmin()
    {
        var admin = db.PbUser
            .First(o => o.UserName == UserName);

        admin.RoleId = (int)Role.Admin;
        await db.SaveChangesAsync();
        await Refresh();
        return this;
    }
}