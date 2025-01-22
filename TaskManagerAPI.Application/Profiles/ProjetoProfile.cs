using AutoMapper;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Profiles
{
    public class ProjetoProfile : Profile
    {
        public ProjetoProfile()
        {
            // Mapeamento entre Projeto e ProjetoViewModel
            CreateMap<Projeto, ProjetoViewModel>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ReverseMap();
        }
    }
}
