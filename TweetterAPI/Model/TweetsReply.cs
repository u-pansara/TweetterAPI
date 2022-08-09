using System.ComponentModel.DataAnnotations;

namespace TweetsAPI.Model
{
    public class TweetsReply
    {
        [Key]
        public int ReplyId { get; set; }
        public string Reply { get; set; }

        public int UserId { get; set; }

        public int TweetId { get; set; }
        public Tweets tweets { get; set; }
        public User user { get; set; }
    }
}
