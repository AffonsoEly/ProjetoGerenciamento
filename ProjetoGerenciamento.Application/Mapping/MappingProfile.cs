using AutoMapper;
using ProjetoGerenciamento.Domain.Entities;
using ProjetoGerenciamento.Application.DTOs;

namespace ProjetoGerenciamento.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Projeto, ProjetoDto>().ReverseMap();
        }
    }
}
