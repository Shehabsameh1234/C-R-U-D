using Demo.BLl.Interfacies;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLl.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
       
        public EmployeeRepository(AppDbConText dbConText) : base(dbConText)
        {
            
        }
        public IQueryable<Employee> GetEmpByAddress(string address)
        {
            return _dbcontext.Employees.Where(e=>e.Address.ToLower().Contains(address.ToLower()));
        }

        public IQueryable<Employee> GetEmpByName(string name)
        {
           
             return _dbcontext.Employees.Where(e => e.Name.ToLower().Contains(name)).Include(e=>e.Department); 
            
           
        }
    }
}
