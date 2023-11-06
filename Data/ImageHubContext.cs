using ImageHubAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHubAPI.Data
{
    /// <summary>
    /// Database context for the ImageHubAPI
    /// </summary>
    public class ImageHubContext : IdentityUserContext<User>, IImageHubContext
  {
    public ImageHubContext(DbContextOptions<ImageHubContext> options) : base(options) { }
    public ImageHubContext() { }

    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Friendship> Friendships { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new FriendshipEntityTypeConfiguration());
      builder.ApplyConfiguration(new UserEntityTypeConfiguration());
      builder.ApplyConfiguration(new ImageEntityTypeConfiguration());

      builder.Entity<IdentityUserToken<string>>()
        .HasKey(d => new { d.UserId, d.LoginProvider, d.Name });
      builder.Entity<IdentityUserLogin<string>>()
        .HasKey(d => new { d.LoginProvider, d.ProviderKey });
    }

    /// <summary>
    /// Configure User Entity
    /// </summary>
    private sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
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
              PasswordHash = new PasswordHasher<User>().HashPassword(null, "passworD1!")
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
              PasswordHash = new PasswordHasher<User>().HashPassword(null, "passworD1!")
            }
          );
      }
    }

    /// <summary>
    /// Configure Friendship Entity
    /// </summary>
    private sealed class FriendshipEntityTypeConfiguration : IEntityTypeConfiguration<Friendship>
    {
      public void Configure(EntityTypeBuilder<Friendship> builder)
      {
        builder
          .HasOne(f => f.FirstUser)
          .WithMany()
          .HasForeignKey(f => f.UserSenderId);

        builder
          .HasOne(f => f.SecondUser)
          .WithMany()
          .HasForeignKey(f => f.FriendId);
      }
    }

    /// <summary>
    /// Configure Image Entity
    /// </summary>
    private sealed class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
    {
      public void Configure(EntityTypeBuilder<Image> builder)
      {
        builder
          .HasIndex(i => i.Path)
          .IsUnique();

        builder
          .HasData(
            new Image
            {
              ImageId = Guid.NewGuid().ToString(),
              Title = "github.png",
              Path = "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/github.png",
              UserId = "23ad2a4f-c1f0-4abc-94c0-52854af2039e"
            },
            new Image
            {
              ImageId = Guid.NewGuid().ToString(),
              Title = "logo.jpg",
              Path = "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/logo.jpg",
              UserId = "23ad2a4f-c1f0-4abc-94c0-52854af2039e"
            },
            new Image
            {
              ImageId = Guid.NewGuid().ToString(),
              Title = "PngItem_6631012.png",
              Path = "/55d8220f-2967-4342-8f6c-e6294a3e52c2/PngItem_6631012.png",
              UserId = "55d8220f-2967-4342-8f6c-e6294a3e52c2"
            },
            new Image
            {
              ImageId = Guid.NewGuid().ToString(),
              Title = "man-search-for-hiring-job-online-from-laptop_1150-52728.jpg",
              Path = "/55d8220f-2967-4342-8f6c-e6294a3e52c2/man-search-for-hiring-job-online-from-laptop_1150-52728.jpg",
              UserId = "55d8220f-2967-4342-8f6c-e6294a3e52c2"
            }
          );
      }
    }
  }
}
