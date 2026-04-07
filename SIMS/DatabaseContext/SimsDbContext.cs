using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext.Entities;

namespace SIMS.DatabaseContext
{
    public class SimsDbContext : DbContext
    {
        public SimsDbContext(DbContextOptions<SimsDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        // 1. ADD THIS LINE
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Existing User configurations...
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey("Id");
            modelBuilder.Entity<User>().HasIndex("Username").IsUnique();
            modelBuilder.Entity<User>().HasIndex("Email").IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue("Admin");
            modelBuilder.Entity<User>().Property(u => u.Status).HasDefaultValue(1);

            // 2. ADD THESE COURSE CONFIGURATIONS
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Course>().HasKey("Id");
            modelBuilder.Entity<Course>().HasIndex("CourseCode").IsUnique(); // Ensure no duplicate course codes
            modelBuilder.Entity<Course>().Property(c => c.Status).HasDefaultValue(1);
            modelBuilder.Entity<Course>().Property(c => c.Enrolled).HasDefaultValue(0);

            // 3. ENROLLMENT CONFIGURATIONS
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
            modelBuilder.Entity<Enrollment>().HasKey(e => e.Id);

            // Define relationships (Foreign Keys)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany() // Or u.Enrollments if you add a collection to User
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany() // Or c.Enrollments if you add a collection to Course
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. GRADE CONFIGURATIONS
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<Grade>().HasKey(g => g.Id);

            // Map relationships for Grades
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Course)
                .WithMany()
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
