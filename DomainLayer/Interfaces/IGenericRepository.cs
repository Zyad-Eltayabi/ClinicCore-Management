using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);  
        T Add(T entity);
        T Update(T entity);
        T Delete(T entity);
        IEnumerable<T> GetAll();
    }
}
