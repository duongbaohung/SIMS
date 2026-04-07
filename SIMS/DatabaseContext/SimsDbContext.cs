using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext.Entities;

namespace SIMS.DatabaseContext
{
    public class SimsDbContext : DbContext
    {
        public SimsDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey("Id");
            modelBuilder.Entity<User>().HasIndex("Username").IsUnique();
            modelBuilder.Entity<User>().HasIndex("Email").IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue("Admin");
            modelBuilder.Entity<User>().Property(u => u.Status).HasDefaultValue(1);
        }
    }
}
