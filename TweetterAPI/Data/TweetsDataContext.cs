using Microsoft.EntityFrameworkCore;
using TweetsAPI.Model;

namespace TweetsAPI.Data
{
    public class TweetsDataContext : DbContext
    {
        public TweetsDataContext(DbContextOptions<TweetsDataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Tweets> Tweets { get; set; }
        public DbSet<TweetsLike> TweetsLikes { get; set; }
        public DbSet<TweetsReply> TweetsReplies { get; set; }

    }
}
