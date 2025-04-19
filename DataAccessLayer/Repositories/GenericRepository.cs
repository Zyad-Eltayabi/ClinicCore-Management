using System.Linq.Expressions;
using DomainLayer.BaseClasses;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _entity;
        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _entity = _context.Set<T>();
        }

        protected readonly ApplicationDbContext _context;


        public void Add(T entity)
        {
            _entity.Add(entity);
        }

        public void Delete(T entity)
        {
            _entity.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _entity.ToList();
        }

        public T GetById(int id)
        {
            return _entity.Find(id);
        }

        public void Update(T entity)
        {
            _entity.Update(entity);
        }

        Result<T> IGenericRepository<T>.Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var rowsAffected = _entity.Where(predicate).ExecuteDelete();

                if (rowsAffected > 0)
                {
                    return Result<T>.Success("Delete operation completed.");
                }
                return Result<T>.Failure("No changes were saved to the database.");
            }
            catch (Exception e)
            {
                return Result<T>.Failure(e.Message);
            }
        }
    }
}
