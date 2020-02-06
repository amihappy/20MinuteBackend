using _20MinuteBackend.Domain.Backend;
using Microsoft.EntityFrameworkCore;

namespace _20MinuteBackend.Infrastructure
{
    public class BackendDbContext : DbContext
    {
        public DbSet<Backend> Backends { get; set; }

        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BackendEntityTypeConfiguration());
        }
    }
}
