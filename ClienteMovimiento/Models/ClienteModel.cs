namespace ClienteMovimiento.Models
{
    public class ClienteModel : PersonaModel
    {

        public int ClienteId { get; set; }
        public string Contrasena { get; set; }
        public string Estado { get; set; }
        public List<CuentaModel> Cuentas { get; set; }

    }
}
