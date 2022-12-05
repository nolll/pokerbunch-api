using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;
using Dapper;

namespace Infrastructure.Sql.SqlDb;

public class SqlUserDb
{
    private const string DataSql = @"
        SELECT u.user_id, u.user_name, u.display_name, u.real_name, u.email, u.password, u.salt, u.role_id
        FROM pb_user u ";

    private const string SearchSql = @"
        SELECT u.user_id
        FROM pb_user u ";
        
    private readonly IDb _db;

    public SqlUserDb(IDb db)
    {
        _db = db;
    }

    public async Task<User> Get(string id)
    {
        var sql = string.Concat(DataSql, "WHERE u.user_id = @userId");
        
        var @params = new
        {
            userId = int.Parse(id)
        };

        var rawUser = await _db.Single<RawUser>(sql, @params);
        return rawUser != null ? RawUser.CreateReal(rawUser) : null;
    }

    public async Task<IList<User>> Get(IList<string> ids)
    {
        var whereClause = _db.Engine == DbEngine.Postgres
            ? "WHERE u.user_id = ANY (@ids)"
            : "WHERE u.user_id IN (@ids)";
        var sql = string.Concat(DataSql, whereClause);

        var @params = new
        {
            ids = ids.Select(int.Parse).ToArray()
        };

        var rawUsers = await _db.List<RawUser>(sql, @params);
        return rawUsers.Select(RawUser.CreateReal).OrderBy(o => o.DisplayName).ToList();
    }

    public async Task<IList<string>> Find()
    {
        return await GetIds();
    }

    public async Task<string> Find(string nameOrEmail)
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
}