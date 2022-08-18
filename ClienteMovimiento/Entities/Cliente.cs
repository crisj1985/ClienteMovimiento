using System;
using System.ComponentModel.DataAnnotations;

namespace ClienteMovimiento.Entities
{
    public class Cliente : Persona
    {
        public Cliente()
        {
        }
        public string Contrasena { get; set; }
        public string Estado { get; set; }
        public List<Cuenta> Cuentas { get; set; }
    }
}