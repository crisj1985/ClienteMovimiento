
using System;
namespace ClienteMovimiento.Entities
{
    public class Persona
    {
        public Persona()
        {
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Telef1ono { get; set; }
    }
}