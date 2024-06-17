using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Configuration
{
    public class EmployeeCofiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Salary)
                    .HasColumnType("decimal(18,2)");
            builder.HasOne(e => e.Department)
                    .WithMany(e => e.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .IsRequired(false).OnDelete(DeleteBehavior.SetNull);

            builder.Property(e => e.Name)
                   .IsRequired(true)
                   .HasMaxLength(50);
        }
    }
}
