using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Repositories
{
    public class Repository<T> : IRepository<T> where T : class    
    {
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public AppDbContext _context { get; }

        public async Task Add(T Entity)
        {
            if (Entity == null)
            {
                throw new ArgumentNullException(nameof(Entity),"La entidad no puede ser null");
            }
            _context.Add(Entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T Entity)
        {
            if (Entity == null)
            {
                throw new ArgumentNullException(nameof(Entity), "La entidad no puede ser null");
            }
            _context.Remove(Entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T Entity)
        {
            if (Entity == null)
            {
                throw new ArgumentNullException(nameof(Entity), "La entidad no puede ser null");
            }
            _context.Update(Entity);
            await _context.SaveChangesAsync();
        }
        public  DbSet<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetElementById(int id)
        {
            return _context.Set<T>().Find(id);
        }

    }
}
