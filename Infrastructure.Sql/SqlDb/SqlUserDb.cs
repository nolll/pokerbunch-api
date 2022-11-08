using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb;

public class SqlUserDb
{
    private const string DataSql = @"
SELECT u.user_iD, u.user_name, u.display_name, u.real_name, u.email, u.password, u.salt, u.role_id
FROM pb_user u ";

    private const string SearchSql = @"
SELECT u.user_id
FROM pb_user u ";
        
    private readonly PostgresStorageProvider _db;

    public SqlUserDb(PostgresStorageProvider db)
    {
        _db = db;
    }

    public User Get(int id)
    {
        var sql = string.Concat(DataSql, "WHERE u.user_id = @userId");
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@userId", id)
        };
        var reader = _db.Query(sql, parameters);
        var rawUser = reader.ReadOne(CreateRawUser);
        return rawUser != null ? RawUser.CreateReal(rawUser) : null;
    }

    public IList<User> Get(IList<int> ids)
    {
        var sql = string.Concat(DataSql, "WHERE u.user_id IN(@ids)");
        var parameter = new ListSqlParameter("@ids", ids);
        var reader = _db.Query(sql, parameter);
        var rawUsers = reader.ReadList(CreateRawUser);
        return rawUsers.Select(RawUser.CreateReal).OrderBy(o => o.DisplayName).ToList();
    }

    public IList<int> Find()
    {
        return GetIds();
    }

    public int Find(string nameOrEmail)
    {
        var userId = GetIdByNameOrEmail(nameOrEmail);
        return userId ?? 0;
    }

    public void Update(User user)
    {
        const string sql = @"
UPDATE pb_user 
SET display_name = @displayName,
    real_name = @realName,
    email = @email,
    password = @password,
    salt = @salt
WHERE user_id = @userId";
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@displayName", user.DisplayName),
            new SimpleSqlParameter("@realName", user.RealName),
            new SimpleSqlParameter("@email", user.Email),
            new SimpleSqlParameter("@password", user.EncryptedPassword),
            new SimpleSqlParameter("@salt", user.Salt),
            new SimpleSqlParameter("@userId", user.Id)
        };
        _db.Execute(sql, parameters);
    }

    public int Add(User user)
    {
        const string sql = @"
INSERT INTO pb_user (user_name, display_name, email, role_id, password, salt)
VALUES (@userName, @displayName, @email, 1, @password, @salt) RETURNING user_id";
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@userName", user.UserName),
            new SimpleSqlParameter("@displayName", user.DisplayName),
            new SimpleSqlParameter("@email", user.Email),
            new SimpleSqlParameter("@password", user.EncryptedPassword),
            new SimpleSqlParameter("@salt", user.Salt)
        };
        return _db.ExecuteInsert(sql, parameters);
    }
        
    private int? GetIdByNameOrEmail(string nameOrEmail)
    {
        if (string.IsNullOrEmpty(nameOrEmail))
            return null;

        var sql = string.Concat(SearchSql, "WHERE (u.user_name = @query OR u.email = @query)");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@query", nameOrEmail)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadInt("user_id");
    }

    private IList<int> GetIds()
    {
        var sql = string.Concat(SearchSql, "ORDER BY u.display_name");
        var reader = _db.Query(sql);
        return reader.ReadIntList("user_id");
    }

    public bool DeleteUser(int userId)
    {
        const string sql = "DELETE FROM pb_user WHERE user_iD = @userId";
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@userId", userId)
        };
        var rowCount = _db.Execute(sql, parameters);
        return rowCount > 0;
    }

    private static RawUser CreateRawUser(IStorageDataReader reader)
    {
        return new RawUser(
            reader.GetIntValue("user_id"),
            reader.GetStringValue("user_name"),
            reader.GetStringValue("display_name"),
            reader.GetStringValue("real_name"),
            reader.GetStringValue("email"),
            reader.GetIntValue("role_id"),
            reader.GetStringValue("password"),
            reader.GetStringValue("salt"));
    }
}