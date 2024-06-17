using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLl.Interfacies
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
       IQueryable<Employee> GetEmpByAddress(string address);
       IQueryable<Employee> GetEmpByName(string name);

    }
}
