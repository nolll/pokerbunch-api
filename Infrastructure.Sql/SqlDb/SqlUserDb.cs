using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;

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
        var sql = string.Concat(DataSql, "WHERE u.user_id IN (@ids)");

        var @params = new ListParam("@ids", ids.Select(int.Parse));

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
        
        var @params = new
        {
            displayName = user.DisplayName,
            realName = user.RealName,
            email = user.Email,
            password = user.EncryptedPassword,
            salt = user.Salt,
            userId = int.Parse(user.Id)
        };

        await _db.Execute(sql, @params);
    }

    public async Task<string> Add(User user)
    {
        const string sql = @"
            INSERT INTO pb_user (user_name, display_name, email, role_id, password, salt)
            VALUES (@userName, @displayName, @email, 1, @password, @salt) RETURNING user_id";

        var @params = new
        {
            userName = user.UserName,
            displayName = user.DisplayName,
            email = user.Email,
            password = user.EncryptedPassword,
            salt = user.Salt
        };

        return (await _db.Insert(sql, @params)).ToString();
    }
        
    private async Task<string> GetIdByNameOrEmail(string nameOrEmail)
    {
        if (string.IsNullOrEmpty(nameOrEmail))
            return null;

        var sql = string.Concat(SearchSql, "WHERE (u.user_name = @query OR u.email = @query)");

        var @params = new
        {
            query = nameOrEmail
        };

        return (await _db.Single<int?>(sql, @params))?.ToString();
    }

    private async Task<IList<string>> GetIds()
    {
        var sql = string.Concat(SearchSql, "ORDER BY u.display_name");
        return (await _db.List<int>(sql)).Select(o => o.ToString()).ToList();
    }

    public async Task<bool> DeleteUser(string userId)
    {
        const string sql = @"
            DELETE FROM pb_user
            WHERE user_iD = @userId";

        var @params = new
        {
            userId = int.Parse(userId)
        };

        var rowCount = await _db.Execute(sql, @params);
        return rowCount > 0;
    }
}