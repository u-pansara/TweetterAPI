using System.ComponentModel.DataAnnotations;

namespace TweetsAPI.Model
{
    public class Tweets
    {
        [Key]
        public int TweetId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }  = DateTime.Now;

        public int UserId { get; set; }


    }
}
