using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;

namespace ClienteMovimiento.Mappers
{
    public class MovimientoMapperProfile : Profile
    {
        public MovimientoMapperProfile()
        {
            CreateMap<Movimiento, MovimientoModel>().ReverseMap();

            CreateMap<Movimiento, MovimientoActualizarModel>().ReverseMap();
        }
    }
}
