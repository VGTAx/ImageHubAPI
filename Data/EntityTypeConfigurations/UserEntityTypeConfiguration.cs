using ImageHubAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHubAPI.Data.EntityTypeConfigurations
{
    /// <summary>
    /// Configure User Entity
    /// </summary>  
    public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.UserImages)
              .WithOne(i => i.User)
              .HasForeignKey(i => i.UserId);

            builder.Metadata
              .FindNavigation(nameof(User.UserImages))!
              .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
              .FindNavigation(nameof(User.Friendships))!
              .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder
              .HasData(
                new User
                {
                    Id = "55d8220f-2967-4342-8f6c-e6294a3e52c2",
                    Name = "Username_1",
                    Email = "username_1@example.com",
                    UserName = "username_1@example.com",
                    NormalizedEmail = "USERNAME_1@EXAMPLE.COM",
                    NormalizedUserName = "USERNAME_1@EXAMPLE.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = true,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "passworD1!")
                },
                new User
                {
                    Id = "23ad2a4f-c1f0-4abc-94c0-52854af2039e",
                    Name = "Username_2",
                    Email = "username_2@example.com",
                    UserName = "username_2@example.com",
                    NormalizedEmail = "USERNAME_2@EXAMPLE.COM",
                    NormalizedUserName = "USERNAME_2@EXAMPLE.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = true,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "passworD1!")
                }
              );
        }
    }
}
