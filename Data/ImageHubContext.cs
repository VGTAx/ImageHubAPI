using ImageHubAPI.Data.EntityTypeConfigurations;
using ImageHubAPI.Interfaces;
using ImageHubAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ImageHubAPI.Data
{
    /// <summary>
    /// Database context for the ImageHubAPI
    /// </summary>
    public class ImageHubContext : IdentityUserContext<User>, IImageHubContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageHubContext(DbContextOptions<ImageHubContext> options) : base(options) { }

        /// <summary>
        /// 
        /// </summary>
        public ImageHubContext() { }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Image> Images { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Friendship> Friendships { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public new EntityEntry Update<TEntity>(TEntity entity)
        {
            return base.Update(entity);
        }
    }
}
