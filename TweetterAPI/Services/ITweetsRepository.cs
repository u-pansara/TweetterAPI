using TweetsAPI.Model;

namespace TweetsAPI.Services
{
    public interface ITweetsRepository
    {
        Task<ServiceResponse<List<Tweets>>> GetAllTweets();
        Task<ServiceResponse<List<Tweets>>> GetAllTweetsforUser(string userName);
        Task<ServiceResponse<Tweets>> GetTweetsById(int id);
        Task<ServiceResponse<int>> Add(Tweets Tweets);
        Task<ServiceResponse<int>> Update(int id, string TweetsDescription);
        Task<ServiceResponse<int>> Delete(int id);
        Task<ServiceResponse<int>> Reply(int id, string reply, int userId);
        Task<ServiceResponse<int>> Like(int id, bool flag, int userId);

    }
}
