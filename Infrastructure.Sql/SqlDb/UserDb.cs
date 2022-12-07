using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Sql;

namespace Infrastructure.Sql.SqlDb;

public class UserDb
{
    private readonly IDb _db;

    public UserDb(IDb db)
    {
        _db = db;
    }

    public async Task<User> Get(string id)
    {
        var @params = new
        {
            userId = int.Parse(id)
        };

        var rawUser = await _db.Single<UserDto>(UserSql.GetByIdQuery, @params);
        return rawUser != null ? UserDto.CreateReal(rawUser) : null;
    }

    public async Task<IList<User>> Get(IList<string> ids)
    {
        var @params = new ListParam("@ids", ids.Select(int.Parse));

        var rawUsers = await _db.List<UserDto>(UserSql.GetByIdsQuery, @params);
        return rawUsers.Select(UserDto.CreateReal).OrderBy(o => o.DisplayName).ToList();
    }

    public async Task<IList<string>> Find()
    {
        return await GetIds();
    }

    public async Task<string> FindByUserName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var @params = new
        {
            name = name
        };

        return (await _db.Single<int?>(UserSql.FindByUsernameQuery, @params))?.ToString();
    }

    public async Task<string> FindByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return null;

        var @params = new
        {
            email = email
        };

        return (await _db.Single<int?>(UserSql.FindByEmailQuery, @params))?.ToString();
    }

    public async Task<string> FindByUserNameOrEmail(string nameOrEmail)
    {
        if (string.IsNullOrEmpty(nameOrEmail))
            return null;

        var @params = new
        {
            query = nameOrEmail
        };

        return (await _db.Single<int?>(UserSql.FindByUsernameOrEmailQuery, @params))?.ToString();
    }

    public async Task Update(User user)
    {
        var @params = new
        {
            displayName = user.DisplayName,
            realName = user.RealName,
            email = user.Email,
            password = user.EncryptedPassword,
            salt = user.Salt,
            userId = int.Parse(user.Id)
        };

        await _db.Execute(UserSql.UpdateQuery, @params);
    }

    public async Task<string> Add(User user)
    {
        var @params = new
        {
            userName = user.UserName,
            displayName = user.DisplayName,
            email = user.Email,
            password = user.EncryptedPassword,
            salt = user.Salt
        };

        return (await _db.Insert(UserSql.AddQuery, @params)).ToString();
    }

    private async Task<IList<string>> GetIds()
    {
        return (await _db.List<int>(UserSql.FindAllQuery)).Select(o => o.ToString()).ToList();
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var @params = new
        {
            userId = int.Parse(userId)
        };

        var rowCount = await _db.Execute(UserSql.DeleteQuery, @params);
        return rowCount > 0;
    }
}