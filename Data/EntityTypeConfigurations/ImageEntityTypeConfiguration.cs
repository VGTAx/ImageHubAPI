using ImageHubAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHubAPI.Data.EntityTypeConfigurations
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
    {
        /// <summary>
        /// Configure Image Entity
        /// </summary>
        /// <param name="builder"></param>
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
