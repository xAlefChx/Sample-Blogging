using AhCh.Posts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AhCh.Posts.Configurations
{
    public class PostCommentConfigurations : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.HasKey(pc => new { pc.PostId, pc.CommentId });

            builder.HasOne(pc => pc.Post)
                   .WithMany(p => p.PostComments)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pc => pc.Comment)
                   .WithMany(c => c.PostComments)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}