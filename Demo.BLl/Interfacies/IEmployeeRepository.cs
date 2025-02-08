using Demo.DAL.Models;
using System.Linq;


namespace Demo.BLl.Interfacies
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
       IQueryable<Employee> GetEmpByAddress(string address);
       IQueryable<Employee> GetEmpByName(string name);

    }
}
