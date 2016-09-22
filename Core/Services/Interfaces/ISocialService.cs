using Core.Entities;

namespace Core.Services
{
	public interface ISocialService
    {
    	void ShareResult(User user, int amount);
	}
}