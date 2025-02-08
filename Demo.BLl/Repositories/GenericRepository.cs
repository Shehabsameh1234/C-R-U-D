using Demo.BLl.Interfacies;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace Demo.BLl.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbConText _dbcontext;
        public GenericRepository(AppDbConText dbConText)
        {
            _dbcontext = dbConText;
        }
        public void Add(T entity)
        {
            _dbcontext.Add(entity);
            
        }

        public void Delate(T entity)
        {
            _dbcontext.Remove(entity);
           
        }
        public void Update(T entity)
        {
            _dbcontext.Update(entity);
           
        }

        public T GetById(int id)
        {
          
            return _dbcontext.Set<T>().Find(id); 
        }
        public IEnumerable<T> GetAll()
        {
            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>)_dbcontext.Employees.Include(e => e.Department).AsNoTracking().ToList();
            }
            else
            {
                return _dbcontext.Set<T>().AsNoTracking().ToList();
            }
              

        }
    }
}
