using AhCh.Posts.Configurations;
using AhCh.Posts.Entities;
using AhCh.Users.Configurations;
using AhCh.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace AhCh.Data
{
    public class AhChContext : DbContext
    {
        public AhChContext(DbContextOptions<AhChContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new PostConfigurations());
            modelBuilder.ApplyConfiguration(new CommentConfigurations());
            modelBuilder.ApplyConfiguration(new PostCommentConfigurations());
        }
    }
}