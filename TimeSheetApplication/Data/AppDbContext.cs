using Microsoft.EntityFrameworkCore;
using TimeSheetApplicaiton.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeSheetApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<User>(u => u.EmployeeId);

            modelBuilder.Entity<Contractor>()
                .HasMany(c => c.Jobs)
                .WithOne(j => j.Contractor)
                .HasForeignKey(j => j.ContractorId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Timesheets)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId);

            modelBuilder.Entity<Job>()
                .HasMany(j => j.Timesheets)
                .WithOne(t => t.Job)
                .HasForeignKey(t => t.JobId);

            modelBuilder.Entity<Timesheet>()
               .Property(t => t.Mileage)
               .HasColumnType("decimal(18, 2)"); 

            modelBuilder.Entity<Employee>()
                .Property(e => e.Wage)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.Date)
                .HasColumnType("date");
        }
    }
}
