using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
