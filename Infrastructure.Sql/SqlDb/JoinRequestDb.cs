using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class JoinRequestDb(IDb db)
{
    public async Task<string> Add(JoinRequest joinRequest)
    {
        var sql = $"""
                   INSERT INTO {Schema.JoinRequest} 
                   (
                     {Schema.JoinRequest.BunchId.AsParam()}, 
                     {Schema.JoinRequest.UserId.AsParam()}
                   )
                   VALUES
                   (
                     (SELECT {Schema.Bunch.Id} FROM {Schema.Bunch} WHERE {Schema.Bunch.Name} = @{Schema.Bunch.Slug.AsParam()}), 
                     @{Schema.JoinRequest.UserId.AsParam()}
                   )
                   RETURNING {Schema.JoinRequest.Id.AsParam()}
                   """;

        var parameters = new Dictionary<string, object?>
        {
            { Schema.Bunch.Slug.AsParam(), joinRequest.BunchId },
            { Schema.JoinRequest.UserId.AsParam(), joinRequest.UserId }
        };
        
        var result = await db.CustomInsert(sql, parameters);
        return result.ToString();
    }
}