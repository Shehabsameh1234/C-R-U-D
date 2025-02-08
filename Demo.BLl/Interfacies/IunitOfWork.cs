
namespace Demo.BLl.Interfacies
{
    public interface IunitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IdepartmentRepository DepartmentRepository { get; set; }

        int Copmlete();
    }
}
