using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class UserDb(PokerBunchDbContext db)
{
    public async Task<User> Get(string id)
    {
        var users = await Get([id]);
        return users.Count > 0 ? users.First() : throw new PokerBunchException($"User with id {id} was not found");
    }

    public async Task<IList<User>> Get(IList<string> ids)
    {
        var query = db.PbUser
            .Where(o => ids.Select(int.Parse).Contains(o.UserId))
            .Select(o => new UserDto
            {
                UserId = o.UserId,
                UserName = o.UserName,
                DisplayName = o.DisplayName,
                RealName = o.RealName,
                Email = o.Email,
                RoleId = o.RoleId,
                Password = o.Password,
                Salt = o.Salt
            });

        var userDtos = await query.ToListAsync();
        return userDtos.Select(UserMapper.ToUser).OrderBy(o => o.DisplayName).ToList();
    }

    public async Task<IList<string>> Find() => await GetIds();

    public async Task<string?> FindByUserName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var query = db.PbUser
            .Where(o => o.UserName == name)
            .Select(o => (int?)o.UserId);

        var result = await query.FirstOrDefaultAsync();
        return result?.ToString();
    }

    public async Task<string?> FindByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return null;

        var query = db.PbUser
            .Where(o => o.Email == email)
            .Select(o => (int?)o.UserId);
        
        var result = await query.FirstOrDefaultAsync();
        return result?.ToString();
    }

    public async Task<string?> FindByUserNameOrEmail(string nameOrEmail)
    {
        if (string.IsNullOrEmpty(nameOrEmail))
            return null;
        
        var query = db.PbUser
            .Where(o => o.Email == nameOrEmail || o.UserName == nameOrEmail)
            .Select(o => (int?)o.UserId);
        
        var result = await query.FirstOrDefaultAsync();
        return result?.ToString();
    }

    public async Task Update(User user)
    {
        var dto = db.PbUser
            .First(o => o.UserId == int.Parse(user.Id));

        dto.DisplayName = user.DisplayName;
        dto.RealName = user.RealName;
        dto.Email = user.Email;
        dto.Password = user.EncryptedPassword;
        dto.Salt = user.Salt;

        await db.SaveChangesAsync();
    }

    public async Task<string> Add(User user)
    {
        var dto = new PbUser
        {
            UserName = user.UserName,
            DisplayName = user.DisplayName,
            RoleId = (int)Role.Player,
            Email = user.Email,
            Password = user.EncryptedPassword,
            Salt = user.Salt
        };

        db.PbUser.Add(dto);
        await db.SaveChangesAsync();
        return dto.UserId.ToString();
    }

    private async Task<IList<string>> GetIds()
    {
        var query = db.PbUser
            .OrderBy(o => o.DisplayName)
            .Select(o => o.UserId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<bool> DeleteUser(string userId)
    {
        db.PbUser.Remove(new PbUser { UserId = int.Parse(userId) });
        var rowCount = await db.SaveChangesAsync();
        return rowCount > 0;
    }
}