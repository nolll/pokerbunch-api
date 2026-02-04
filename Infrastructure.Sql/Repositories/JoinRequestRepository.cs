using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class JoinRequestRepository(IDb db, ICache cache) : IJoinRequestRepository
{
    private readonly JoinRequestDb _playerDb = new(db);
    
    public Task<string> Add(JoinRequest joinRequest)
    {
        return _playerDb.Add(joinRequest);
    }
}