
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Demo.DAL.Data
{
    public class AppDbConText:IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public AppDbConText(DbContextOptions<AppDbConText> options):base(options) 
        {
            
        }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        

    }
}
