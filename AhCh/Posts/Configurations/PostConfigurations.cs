using AhCh.Posts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AhCh.Posts.Configurations
{
    public class PostConfigurations : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(256);
            builder.Property(c => c.Content).IsRequired().HasColumnType("NVARCHAR(MAX)");

            builder.HasMany(c => c.PostComments).WithOne(c => c.Post);
        }
    }
}