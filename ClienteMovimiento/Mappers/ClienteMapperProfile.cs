﻿using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;

namespace ClienteMovimiento.Mappers
{
    public class ClienteMapperProfile: Profile
    {
        public ClienteMapperProfile()
        {
            CreateMap<Cliente, ClienteModel>();
            CreateMap<ClienteModel, Cliente>();

            CreateMap<Persona, PersonaModel>()
           .Include<Cliente, ClienteModel>().ReverseMap();

            CreateMap<Cliente, ClienteActualizarModel>();
            CreateMap<ClienteActualizarModel, Cliente>();

            CreateMap<Persona, PersonaActualizarModel>()
          .Include<Cliente, ClienteActualizarModel>().ReverseMap();


            //CreateMap<Cliente, ClienteModel>();
        }
    }
}
