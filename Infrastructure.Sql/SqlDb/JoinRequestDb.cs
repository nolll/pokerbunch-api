using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class JoinRequestDb(IDb db)
{
    private static Query JoinRequestQuery => new(Schema.JoinRequest);
    
    private static Query GetQuery => JoinRequestQuery
        .Select(
            Schema.JoinRequest.Id,
            Schema.JoinRequest.BunchId,
            Schema.JoinRequest.UserId,
            Schema.User.UserName)
        .LeftJoin(Schema.User, Schema.User.Id, Schema.JoinRequest.UserId);
    
    public async Task<IList<string>> Find(string slug)
    {
        var query = FindQuery.Where(Schema.Bunch.Name, slug);
        return (await db.GetAsync<int>(query)).Select(o => o.ToString()).ToList();
    }
    
    private static Query FindQuery => JoinRequestQuery
        .Select(Schema.JoinRequest.Id)
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.JoinRequest.BunchId);
    
    public async Task<IList<JoinRequest>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return [];

        var query = GetQuery.WhereIn(Schema.JoinRequest.Id, ids.Select(int.Parse));
        var joinRequestDtos = await db.GetAsync<JoinRequestDto>(query);

        return joinRequestDtos.Select(JoinRequestMapper.ToJoinRequest).ToList();
    }
    
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
            { Schema.JoinRequest.UserId.AsParam(), int.Parse(joinRequest.UserId) }
        };
        
        var result = await db.CustomInsert(sql, parameters);
        return result.ToString();
    }
}