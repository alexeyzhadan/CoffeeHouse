using CoffeeHouse.Data;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;

        public BaseRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public virtual IQueryable<T> GetAll() 
        {
            return _db.Set<T>().AsNoTracking();
        }

        public virtual void Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public virtual async Task AddAsync(T entity)
        {
            await Task.Run(() => Add(entity));
        }

        public virtual void Update(T entity)
        {
            _db.Set<T>().Update(entity);
            _db.SaveChanges();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await Task.Run(() => Update(entity));
        }

        public virtual void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
            _db.SaveChanges();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            await Task.Run(() => Remove(entity));
        }

        public abstract bool Exists(T entity);
    }
}