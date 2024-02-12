using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHubAPI.Data.EntityTypeConfigurations
{
    /// <summary>
    /// Configure Friendship Entity
    /// </summary>
    public sealed class FriendshipEntityTypeConfiguration : IEntityTypeConfiguration<Friendship>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
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
}
