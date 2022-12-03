using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql.SqlDb;

public class SqlUserDb
{
    private const string DataSql = @"
        SELECT u.user_iD, u.user_name, u.display_name, u.real_name, u.email, u.password, u.salt, u.role_id
        FROM pb_user u ";

    private const string SearchSql = @"
        SELECT u.user_id
        FROM pb_user u ";
        
    private readonly PostgresDb _db;

    public SqlUserDb(PostgresDb db)
    {
        _db = db;
    }

    public async Task<User> Get(string id)
    {
        var sql = string.Concat(DataSql, "WHERE u.user_id = @userId");
        var parameters = new List<SqlParam>
        {
            new IntParam("@userId", int.Parse(id))
        };
        var reader = await _db.Query(sql, parameters);
        var rawUser = reader.ReadOne(CreateRawUser);
        return rawUser != null ? RawUser.CreateReal(rawUser) : null;
    }

    public async Task<IList<User>> Get(IList<string> ids)
    {
        var sql = string.Concat(DataSql, "WHERE u.user_id IN(@ids)");
        var parameter = new IntListParam("@ids", ids);
        var reader = await _db.Query(sql, parameter);
        var rawUsers = reader.ReadList(CreateRawUser);
        return rawUsers.Select(RawUser.CreateReal).OrderBy(o => o.DisplayName).ToList();
    }

    public async Task<IList<string>> Find()
    {
        return await GetIds();
    }

    public async Task<string> FindByName(string name)
    {
        return await GetIdByName(name);
    }

    public async Task<string> FindByEmail(string email)
    {
        return await GetIdByEmail(email);
    }

    public async Task<string> FindByNameOrEmail(string nameOrEmail)
    {
        return await GetIdByNameOrEmail(nameOrEmail);
    }

    public async Task Update(User user)
    {
        const string sql = @"
            UPDATE pb_user 
            SET display_name = @displayName,
                real_name = @realName,
                email = @email,
                password = @password,
                salt = @salt
            WHERE user_id = @userId";
        var parameters = new List<SqlParam>
        {
            new StringParam("@displayName", user.DisplayName),
            new StringParam("@realName", user.RealName),
            new StringParam("@email", user.Email),
            new StringParam("@password", user.EncryptedPassword),
            new StringParam("@salt", user.Salt),
            new IntParam("@userId", user.Id)
        };
        await _db.Execute(sql, parameters);
    }

    public async Task<string> Add(User user)
    {
        const string sql = @"
            INSERT INTO pb_user (user_name, display_name, email, role_id, password, salt)
            VALUES (@userName, @displayName, @email, 1, @password, @salt) RETURNING user_id";
        var parameters = new List<SqlParam>
        {
            new StringParam("@userName", user.UserName),
            new StringParam("@displayName", user.DisplayName),
            new StringParam("@email", user.Email),
            new StringParam("@password", user.EncryptedPassword),
            new StringParam("@salt", user.Salt)
        };
        return (await _db.Insert(sql, parameters)).ToString();
    }
        
    private async Task<string> GetIdByNameOrEmail(string nameOrEmail)
    {
        if (string.IsNullOrEmpty(nameOrEmail))
            return null;

        var sql = string.Concat(SearchSql, "WHERE (u.user_name = @query OR u.email = @query)");
        var parameters = new List<SqlParam>
        {
            new StringParam("@query", nameOrEmail)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadInt("user_id")?.ToString();
    }

    private async Task<string> GetIdByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var sql = string.Concat(SearchSql, "WHERE u.user_name = @name");
        var parameters = new List<SqlParam>
        {
            new StringParam("@name", name)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadInt("user_id")?.ToString();
    }

    private async Task<string> GetIdByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return null;

        var sql = string.Concat(SearchSql, "WHERE u.email = @email");
        var parameters = new List<SqlParam>
        {
            new StringParam("@email", email)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadInt("user_id")?.ToString();
    }

    private async Task<IList<string>> GetIds()
    {
        var sql = string.Concat(SearchSql, "ORDER BY u.display_name");
        var reader = await _db.Query(sql);
        return reader.ReadIntList("user_id").Select(o => o.ToString()).ToList();
    }

    public async Task<bool> DeleteUser(string userId)
    {
        const string sql = @"
            DELETE FROM pb_user
            WHERE user_iD = @userId";
        var parameters = new List<SqlParam>
        {
            new IntParam("@userId", userId)
        };
        var rowCount = await _db.Execute(sql, parameters);
        return rowCount > 0;
    }

    private static RawUser CreateRawUser(IStorageDataReader reader)
    {
        return new RawUser(
            reader.GetIntValue("user_id").ToString(),
            reader.GetStringValue("user_name"),
            reader.GetStringValue("display_name"),
            reader.GetStringValue("real_name"),
            reader.GetStringValue("email"),
            reader.GetIntValue("role_id"),
            reader.GetStringValue("password"),
            reader.GetStringValue("salt"));
    }
}