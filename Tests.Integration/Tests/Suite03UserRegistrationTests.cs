using System.Net;
using Api.Models.UserModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.UserRegistration, 1)]
    public async Task Suite03UserRegistration_01RegisterReturns200()
    {
        var parameters = new AddUserPostModel(
            DataFactory.String(),
            DataFactory.String(),
            DataFactory.EmailAddress(),
            DataFactory.String());
        
        var result = await ApiClient.User.Add(parameters);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}