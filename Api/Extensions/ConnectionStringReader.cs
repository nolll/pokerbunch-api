using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Api.Extensions;

public static class ConnectionStringExtensions
{
    public static string BuildConnectionString(this IConfiguration configuration)
    {
        var databaseUrl = configuration.GetValue<string>("DATABASE_URL");
        if (string.IsNullOrEmpty(databaseUrl))
            return "";

        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');
    
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/')
        };

        return connectionStringBuilder.ToString();
    }
}

public static class TokenLifetimeValidator
{
    public static bool Validate(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters param)
    {
        if (expires != null)
            return expires > DateTime.UtcNow;

        return false;
    }

}