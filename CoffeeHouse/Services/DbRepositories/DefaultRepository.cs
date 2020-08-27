using CoffeeHouse.Data;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeHouse.Services.DbRepositories
{
    public abstract class DefaultRepository<T> : IDefaultEntityRepository<T> where T : class, IDefaultEntity
    {
        protected readonly ApplicationDbContext _db;

        public DefaultRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public virtual IQueryable<T> GetAll() 
        {
            return _db.Set<T>().AsNoTracking();
        }

        public virtual T GetById(int id)
        {
            return GetAll().SingleOrDefault(e => e.Id == id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
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

        public virtual bool Exists(T entity)
        {
            return GetAll().Any(e => e.Id == entity.Id);
        }
    }
}