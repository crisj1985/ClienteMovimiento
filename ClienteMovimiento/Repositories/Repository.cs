using ClienteMovimiento.ManejoExcepciones;
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
            try
            {
                if (Entity == null)
                {
                    throw new ArgumentNullException(nameof(Entity),"La entidad no puede ser null");
                }
                _context.Add(Entity);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento($"{ex?.Message} - {ex?.StackTrace}" );
             
            }
        }

        public async Task Delete(T Entity)
        {
            try
            {
                if (Entity == null)
                {
                    throw new ArgumentNullException(nameof(Entity), "La entidad no puede ser null");
                }
                _context.Remove(Entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento($"{ex?.Message} - {ex?.StackTrace}" );
             
            }
}

        public async Task Update(T Entity)
        {
            try
            {
                if (Entity == null)
                {
                    throw new ArgumentNullException(nameof(Entity), "La entidad no puede ser null");
                }
                _context.Update(Entity);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento($"{ex?.Message} - {ex?.StackTrace}");

            }
        }
        public  DbSet<T> GetAll()
        {
            try
            {

               return _context.Set<T>();
            }
            catch(Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento($"{ex?.Message} - {ex?.StackTrace}");
            }
            return null;
        }

        public T GetElementById(int id)
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento($"{ex?.Message} - {ex?.StackTrace}");
            }
            return null;
        }

    }
}
