using Microsoft.EntityFrameworkCore;
using securityservice.Model;

namespace securityservice.Data
{
 public class ApplicationDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<User> ()
                .Property (b => b.enabled)
                .HasDefaultValue (true);
            modelBuilder.Entity<UserGroup> ()
                .Property (b => b.enabled)
                .HasDefaultValue (true);
        }
    }
}