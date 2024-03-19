using GeoTracer.Shared;
using GeoTracer.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace GeoTracer.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions):base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
