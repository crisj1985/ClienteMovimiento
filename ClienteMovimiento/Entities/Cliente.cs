using System;
namespace ClienteMovimiento.Entities
{
    public class Cliente : Persona
    {
        public Cliente()
        {
        }
        public int ClienteId { get; set; }
        public int CienteId { get; set; }
        public string Contrasena { get; set; }
        public string Estado { get; set; }
    }
}