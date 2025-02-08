using Demo.BLl.Interfacies;
using Demo.DAL.Data;
using Demo.DAL.Models;


namespace Demo.BLl.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IdepartmentRepository
    {
        
        public DepartmentRepository(AppDbConText dbConText) : base(dbConText)
        {
           
        }
    }
}
