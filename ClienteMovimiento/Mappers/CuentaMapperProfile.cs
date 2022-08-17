using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;

namespace ClienteMovimiento.Mappers
{
    public class CuentaMapperProfile:Profile
    {
        public CuentaMapperProfile()
        {
            CreateMap<Cuenta,CuentaModel>().ReverseMap();
            CreateMap<Cuenta,CuentaActualizarModel>().ReverseMap();
        }
    }
}
