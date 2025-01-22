using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Profiles
{
    public class UsuarioProfiles : Profile
    {

        // estou usando autommaper mais com exemplo de uso, para entendimento das camadas
        public UsuarioProfiles()
        {
            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ReverseMap();
        }
    }
}
