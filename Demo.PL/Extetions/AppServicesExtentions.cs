using Demo.BLl.Interfacies;
using Demo.BLl.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL.Extetions
{
    public static class AppServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IdepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}
