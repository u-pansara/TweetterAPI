using TweetsAPI.Data;
using TweetsAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace TweetsAPI.Services
{
    public class TweetsRepository : ITweetsRepository
    {
        private readonly TweetsDataContext _TweetserDataContext;

        public TweetsRepository(TweetsDataContext TweetserDataContext)
        {
            _TweetserDataContext = TweetserDataContext;
        }

        public async Task<ServiceResponse<int>> Add(Tweets Tweets)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            _TweetserDataContext.Add(Tweets);
            await _TweetserDataContext.SaveChangesAsync();

            response.Success = true;
            response.Message = "Tweets posted successfully";
            return response;
        }

        public async Task<ServiceResponse<int>> Delete(int id)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            var Tweets = await _TweetserDataContext.Tweets.FirstOrDefaultAsync(t => t.TweetId == id);
            if (Tweets == null)
            {
                response.Success = false;
                response.Message = "Tweets not found";
                return response;
            }
            _TweetserDataContext.Tweets.Remove(Tweets);
            await _TweetserDataContext.SaveChangesAsync();

            response.Success = true;
            response.Message = "Tweets deleted successfully";
            return response;
        }

        public async Task<ServiceResponse<List<Tweets>>> GetAllTweets()
        {
            var response = new ServiceResponse<List<Tweets>>();
            var Tweets = await _TweetserDataContext.Tweets.ToListAsync();
            response.Data = Tweets;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<List<Tweets>>> GetAllTweetsforUser(string userName)
        {
            var response = new ServiceResponse<List<Tweets>>();
            var user = await _TweetserDataContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            else
            {
                var Tweets = await _TweetserDataContext.Tweets.Where(t => t.UserId == user.Id).ToListAsync();
                response.Data = Tweets;
                response.Success = true;
                return response;
            }
        }

        public async Task<ServiceResponse<Tweets>> GetTweetsById(int id)
        {
            var response = new ServiceResponse<Tweets>();
            var Tweets = await _TweetserDataContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == id);
            response.Data = Tweets;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<int>> Like(int id, bool flag, int userId)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            var tweet = await _TweetserDataContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == id);
            if (tweet == null)
            {
                response.Success = false;
                response.Message = "Tweets not found";
                return response;
            }
            else
            {
                var like = await _TweetserDataContext.TweetsLikes.FirstOrDefaultAsync(l => l.TweetId == tweet.TweetId && l.UserId == userId);
                if (like == null)
                {
                    _TweetserDataContext.TweetsLikes.Add(new TweetsLike { Liked = flag, TweetId = id, UserId = userId });
                }
                else
                {
                    like.Liked = flag;
                    _TweetserDataContext.TweetsLikes.Update(like);
                }
                await _TweetserDataContext.SaveChangesAsync();

                response.Success = true;
                return response;
            }
        }

        public async Task<ServiceResponse<int>> Reply(int id, string reply, int userId)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            var tweet = await _TweetserDataContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == id);
            if (tweet == null)
            {
                response.Success = false;
                response.Message = "Tweets not found";
                return response;
            }
            else
            {
                _TweetserDataContext.TweetsReplies.Add(new TweetsReply { Reply = reply, TweetId = id, UserId = userId });

                await _TweetserDataContext.SaveChangesAsync();

                response.Success = true;
                return response;
            }
        }

        public async Task<ServiceResponse<int>> Update(int id, string TweetsDescription)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            var Tweets = await _TweetserDataContext.Tweets.FirstOrDefaultAsync(t => t.TweetId == id);
            if (Tweets == null)
            {
                response.Success = false;
                response.Message = "Tweets not found";
                return response;
            }
            Tweets.Description = TweetsDescription;

            _TweetserDataContext.Tweets.Update(Tweets);
            await _TweetserDataContext.SaveChangesAsync();

            response.Success = true;
            response.Message = "Tweets updated successfully";
            return response;
        }
    }
}
