using System.ComponentModel.DataAnnotations;

namespace TweetsAPI.Model
{
    public class TweetsLike
    {
        [Key]
        public int Id { get; set; }

        public bool Liked { get; set; } = false;

        public int TweetId { get; set; }

        public int UserId { get; set; }

        public Tweets tweets { get; set; }

        public User user { get; set; }

    }
}
