namespace ClienteMovimiento.Models
{
    public class CuentasConMovimientosModel : CuentaModel
    {
        public List<MovimientoModel> Movimientos { get; set; }
    }
}
