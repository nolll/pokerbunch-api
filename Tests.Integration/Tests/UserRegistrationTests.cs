using System.Net;
using Api.Models.UserModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class UserRegistrationTests(TestFixture fixture) : IntegrationTests2(fixture)
{
    [Fact]
    public async Task Register()
    {
        var parameters = new AddUserPostModel(
            Data.String(),
            Data.String(),
            Data.EmailAddress(),
            Data.String());
        
        var result = await ApiClient.User.Add(parameters);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}