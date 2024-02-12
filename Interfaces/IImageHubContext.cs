using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ImageHubAPI.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IImageHubContext : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Image> Images { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Friendship> Friendships { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry Update<TEntity>(TEntity entity);
    }
}
