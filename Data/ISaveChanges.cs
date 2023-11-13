namespace ImageHubAPI.Data
{
  public interface ISaveChanges
  {
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
