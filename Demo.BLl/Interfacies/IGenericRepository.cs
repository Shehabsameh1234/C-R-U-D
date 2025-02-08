using Demo.DAL.Models;
using System.Collections.Generic;


namespace Demo.BLl.Interfacies
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delate(T entity);
    }
}
