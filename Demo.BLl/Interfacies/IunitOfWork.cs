using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLl.Interfacies
{
    public interface IunitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IdepartmentRepository DepartmentRepository { get; set; }

        int Copmlete();
    }
}
