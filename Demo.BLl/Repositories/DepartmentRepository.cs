using Demo.BLl.Interfacies;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLl.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IdepartmentRepository
    {
        
        public DepartmentRepository(AppDbConText dbConText) : base(dbConText)
        {
           
        }
    }
}
