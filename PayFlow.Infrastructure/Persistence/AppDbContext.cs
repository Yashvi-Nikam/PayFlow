using Microsoft.EntityFrameworkCore;
using PayFlow.Domain.Entities;

namespace PayFlow.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Payroll> Payrolls => Set<Payroll>();
    public DbSet<BranchSettings> BranchSettings => Set<BranchSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Employee>().ToTable("Employee");
        modelBuilder.Entity<Attendance>().ToTable("Attendance");
        modelBuilder.Entity<Payroll>().ToTable("Payroll");
        modelBuilder.Entity<BranchSettings>().ToTable("BranchSettings");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}
