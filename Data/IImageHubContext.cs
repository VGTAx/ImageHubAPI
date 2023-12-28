using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Data
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
    /// <returns></returns>
    int SaveChanges();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
