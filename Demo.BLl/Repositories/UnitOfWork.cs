using Demo.BLl.Interfacies;
using Demo.DAL.Data;
using System;

namespace Demo.BLl.Repositories
{
    public class UnitOfWork : IunitOfWork ,IDisposable
    {
        private readonly AppDbConText _appContext;

        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IdepartmentRepository DepartmentRepository { get ; set ; }
        public UnitOfWork(AppDbConText appContext)
        {
            EmployeeRepository = new EmployeeRepository(appContext);
            DepartmentRepository = new DepartmentRepository(appContext);
           _appContext = appContext;
        }

        public int Copmlete()
        {
           return _appContext.SaveChanges();
        }

       public void Dispose()
        {
            _appContext.Dispose();
        }
    }
}
