using System;
namespace ClienteMovimiento.Entities
{
    public class Cuenta
    {
        public Cuenta()
        {
        }
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public string Estado { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public List<Movimiento> Movimientos { get; set; }
    }
}