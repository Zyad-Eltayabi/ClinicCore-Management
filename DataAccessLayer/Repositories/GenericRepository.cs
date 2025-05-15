using System.Linq.Expressions;
using System.Threading.Tasks;
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
            var items = await _entity.ToListAsync();;
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
