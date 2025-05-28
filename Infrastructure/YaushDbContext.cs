using Microsoft.EntityFrameworkCore;
using YaushServer.Url;

namespace YaushServer.Infrastructure
{
    public class YaushDbContext : DbContext
    {
        public DbSet<ShortenedUrl> Urls { get; set; }

        public YaushDbContext(DbContextOptions<YaushDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>()
                .Property(x => x.Created)
                .HasDefaultValueSql("current_timestamp");
        }
    }
}
