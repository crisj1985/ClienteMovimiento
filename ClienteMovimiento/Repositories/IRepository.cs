using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> GetAll();
        T GetElementById(int id);
        Task Add(T Entity);
        Task Update(T Entity);
        Task Delete(T Entity);
    }
}
