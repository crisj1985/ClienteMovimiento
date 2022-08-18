namespace ClienteMovimiento.Models
{
    public class ClienteActualizarModel : PersonaActualizarModel
    {
        //public int ClienteId { get; set; }
        public string Contrasena { get; set; }
        public string Estado { get; set; }
    }
}
