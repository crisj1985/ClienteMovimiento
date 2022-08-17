namespace ClienteMovimiento.Models
{
    public class CuentaActualizarModel
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public string Estado { get; set; }
        public int ClienteId { get; set; }
    }
}
