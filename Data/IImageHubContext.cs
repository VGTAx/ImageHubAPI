using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Data
{
  public interface IImageHubContext : IDisposable
  {
    DbSet<Image> Images { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Friendship> Friendships { get; set; }
  }
}
