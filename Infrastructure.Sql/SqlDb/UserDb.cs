using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.SqlDb;

public class UserDb
{
    private readonly IDb _db;

    private static Query UserQuery => new(Schema.User);

    private static Query GetQuery => UserQuery
        .Select(
            Schema.User.Id,
            Schema.User.UserName,
            Schema.User.DisplayName,
            Schema.User.RealName,
            Schema.User.Email,
            Schema.User.Password,
            Schema.User.Salt,
            Schema.User.RoleId);

    private static Query FindQuery => UserQuery.Select(Schema.User.Id);

    public UserDb(IDb db)
    {
        _db = db;
    }

    public async Task<User> Get(string id)
    {
        var query = GetQuery.Where(Schema.User.Id, int.Parse(id));
        var userDto = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<UserDto>();
        var user = userDto?.ToUser();

        return user ?? throw new PokerBunchException($"User with id {id} was not found");
    }

    public async Task<IList<User>> Get(IList<string> ids)
    {
        var query = GetQuery.WhereIn(Schema.User.Id, ids.Select(int.Parse));
        var userDtos = await _db.GetAsync<UserDto>(query);
        return userDtos.Select(UserMapper.ToUser).OrderBy(o => o.DisplayName).ToList();
    }

    public async Task<IList<string>> Find()
    {
        return await GetIds();
    }

    public async Task<string?> FindByUserName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var query = FindQuery.Where(Schema.User.UserName, name);
        var result = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<int?>();
        return result?.ToString();
    }

    public async Task<string?> FindByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return null;

        var query = FindQuery.Where(Schema.User.Email, email);
        var result = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<int?>();
        return result?.ToString();
    }

    public async Task<string?> FindByUserNameOrEmail(string nameOrEmail)
    {
        if (string.IsNullOrEmpty(nameOrEmail))
            return null;
        
        var query = FindQuery
            .Where(Schema.User.UserName, nameOrEmail)
            .OrWhere(Schema.User.Email, nameOrEmail);
        var result = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<int?>();
        return result?.ToString();
    }

    public async Task Update(User user)
    {
        var parameters = new Dictionary<string, object>
        {
            { Schema.User.DisplayName.AsParam(), user.DisplayName },
            { Schema.User.RealName.AsParam(), user.RealName },
            { Schema.User.Email.AsParam(), user.Email },
            { Schema.User.Password.AsParam(), user.EncryptedPassword },
            { Schema.User.Salt.AsParam(), user.Salt }
        };

        var query = UserQuery.Where(Schema.User.Id, int.Parse(user.Id));
        await _db.UpdateAsync(query, parameters);
    }

    public async Task<string> Add(User user)
    {
        var parameters = new Dictionary<string, object>
        {
            { Schema.User.UserName.AsParam(), user.UserName },
            { Schema.User.DisplayName.AsParam(), user.DisplayName },
            { Schema.User.RoleId.AsParam(), (int)Role.Player },
            { Schema.User.Email.AsParam(), user.Email },
            { Schema.User.Password.AsParam(), user.EncryptedPassword },
            { Schema.User.Salt.AsParam(), user.Salt }
        };

        var result = await _db.QueryFactory.FromQuery(UserQuery).InsertGetIdAsync<int>(parameters);
        return result.ToString();
    }

    private async Task<IList<string>> GetIds()
    {
        var query = FindQuery.OrderBy(Schema.User.DisplayName);
        var result = await _db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var query = UserQuery.Where(Schema.User.Id, int.Parse(userId));
        var rowCount = await _db.DeleteAsync(query);
        return rowCount > 0;
    }
}