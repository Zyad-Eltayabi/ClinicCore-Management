using System.Linq.Expressions;
using DataAccessLayer.Persistence;
using DomainLayer.Interfaces.Repositories;
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


        public async Task Add(T entity)
        {
            await _entity.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _entity.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var items = await _entity.ToListAsync(); ;
            return items;
        }

        public async Task<T> GetById(int id)
        {
            var item = await _entity.FindAsync(id);
            return item;
        }

        public void Update(T entity)
        {
            _entity.Update(entity);
        }

        public async Task<bool> Delete(Expression<Func<T, bool>> predicate)
        {

            var rowsAffected = await _entity.Where(predicate).ExecuteDeleteAsync();

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}
