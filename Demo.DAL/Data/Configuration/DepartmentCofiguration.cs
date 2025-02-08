using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Data.Configuration
{
    internal class DepartmentCofiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Department> builder)
        {
            
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
        }
    }
}
