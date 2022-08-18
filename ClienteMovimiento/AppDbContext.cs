using ClienteMovimiento.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options):base(options) 
        {

        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        //public DbSet<Movimiento> Movimientos { get; set; }
    }
}
