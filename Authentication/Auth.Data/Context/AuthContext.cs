using Auth.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Context
{
    public class AuthContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
        }

    }
}
