using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Models;

namespace MinimalAPIDemo.Data
{
    public class MinimalContextDb : DbContext
    {
        public MinimalContextDb(DbContextOptions<MinimalContextDb> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Employee>().HasKey(p => p.Id);
            modelBuilder.Entity<Employee>().Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.Document).HasMaxLength(14).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.Department).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.GrossSalary).HasPrecision(8, 2).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.AdmissionDate).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.HealthPlan).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.DentalPlan).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.TransportantionVoucher).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.UpdateAt).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.Active).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
