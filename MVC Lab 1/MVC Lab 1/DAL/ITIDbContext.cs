using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.Models;

namespace MVC_Lab_1.DAL
{
    public class ITIDbContext:DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=MVCLab1;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(d =>
            {
                d.HasData(
                    new Department { DeptId = 10, DeptName ="CS" ,Capacity = 30},
                    new Department { DeptId = 20, DeptName = "OS", Capacity = 31 },
                    new Department { DeptId = 30, DeptName = "Data", Capacity = 32 }

                    );
            });
        }
    }
}
