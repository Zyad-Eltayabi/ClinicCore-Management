using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.BaseClasses;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        public Result<T> Delete(Expression<Func<T, bool>> predicate);
    }
}
