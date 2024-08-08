using AhCh.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AhCh.Users.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(c => c.Username).IsRequired().HasMaxLength(11);
        builder.Property(c => c.Password).IsRequired().HasMaxLength(100);

        builder.HasMany(c => c.Posts).WithOne(c => c.User).HasForeignKey(c => c.UserId);
    }
}