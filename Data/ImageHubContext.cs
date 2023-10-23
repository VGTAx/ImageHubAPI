using Microsoft.EntityFrameworkCore;
using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHubAPI.Data
{
  /// <summary>
  /// Database context for the ImageHubAPI
  /// </summary>
  public class ImageHubContext : DbContext
  {
    public ImageHubContext(DbContextOptions<ImageHubContext> options) : base(options) { }

    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friendship> Friendships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {      
      modelBuilder.ApplyConfiguration(new FriendshipEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new FriendRequestEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
    /// <summary>
    /// Configure User Entity
    /// </summary>
    class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
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
          .FindNavigation(nameof(User.FriendRequests))!
          .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
          .FindNavigation(nameof(User.Friendships))!
          .SetPropertyAccessMode(PropertyAccessMode.Field);
      }
    }
    /// <summary>
    /// Configure Friendship Entity
    /// </summary>
    class FriendshipEntityTypeConfiguration : IEntityTypeConfiguration<Friendship>
    {
      public void Configure(EntityTypeBuilder<Friendship> builder)
      {
        builder
          .HasOne(f => f.FirstUser)
          .WithMany()
          .HasForeignKey(f => f.FirstUserId);

        builder
          .HasOne(f => f.SecondUser)
          .WithMany()
          .HasForeignKey(f => f.SecondUserId);
      }
    }
    /// <summary>
    /// Configure FriendRequest Entity
    /// </summary>
    class FriendRequestEntityTypeConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
      public void Configure(EntityTypeBuilder<FriendRequest> builder)
      {        
        builder
          .HasOne(fr => fr.Requester)
          .WithMany()
          .HasForeignKey(fr => fr.RequesterId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasOne(fr => fr.Receiver)
          .WithMany()
          .HasForeignKey(fr => fr.ReceiverId)
          .OnDelete(DeleteBehavior.Restrict);     
      }
    }
  }
}
