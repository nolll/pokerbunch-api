using Core.Entities;

namespace Core.Repositories;

public interface IJoinRequestRepository
{
    Task<IList<JoinRequest>> Get(IList<string> ids);
    Task<string> Add(JoinRequest joinRequest);
    Task<IList<JoinRequest>> List(string bunchId);
    Task<JoinRequest?> Get(string bunchId, string userId);
    Task Delete(string id);
}