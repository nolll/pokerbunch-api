using Core.Entities;

namespace Core.Repositories;

public interface IJoinRequestRepository
{
    Task<string> Add(JoinRequest joinRequest);
    Task<IList<JoinRequest>> List(string bunchId);
}