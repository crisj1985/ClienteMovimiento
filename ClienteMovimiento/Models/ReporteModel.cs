namespace ClienteMovimiento.Models
{
    public class ReporteModel
    {
        public string Fecha { get; set; } 
        public string Cliente { get; set; } 
        public string NumeroCuenta { get; set; } 
        public string Tipo { get; set; } 
        public string SaldoInicial { get; set; } 
        public string Estado { get; set; } 
        public string Movimiento { get; set; } 
        public string SaldoDisponible { get; set; } 
    }
}
