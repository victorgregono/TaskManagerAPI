using AutoMapper;
using TaskManager.Application.ViewModels;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Profiles
{
    //public class TarefaProfile : Profile
    //{
    //    public TarefaProfile()
    //    {
    //        CreateMap<Tarefa, TarefaViewModel>()
    //            .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
    //            .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
    //            .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DataVencimento))
    //            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (StatusTarefa)src.Status))
    //            .ForMember(dest => dest.Prioridade, opt => opt.MapFrom(src => (PrioridadeTarefa)src.Prioridade))
    //            .ForMember(dest => dest.ProjetoId, opt => opt.MapFrom(src => src.ProjetoId))
    //            .ForMember(dest => dest.Comentarios, opt => opt.MapFrom(src => src.Comentarios))
    //            .ForMember(dest => dest.Historicos, opt => opt.MapFrom(src => src.Historicos))
    //            .ReverseMap();
    //    }
    //}


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Tarefa, TarefaViewModel>().ReverseMap();
                CreateMap<TarefaViewModel, Tarefa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar o mapeamento do Id se necessário
                .ReverseMap();
        }
    }


}
