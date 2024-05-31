// Mapeamento para definir quais classes vão ser mapeadas para o dto

using AutoMapper;
using PGE.DTOs;
using PGE.Models;

namespace PGE.Mappings
{
    public class EntitiesToDTOMappingProfile : Profile
    {
        public EntitiesToDTOMappingProfile()
        {
            // Criar mapeamento para enviar/receber informações do model para dto
            CreateMap<Pessoa, PessoaDTO>();
            CreateMap<Login, LoginDTO>().ReverseMap();
            CreateMap<Processo, ProcessoDTO>().ReverseMap();
            CreateMap<Documento, DocumentoDTO>().ReverseMap();
            CreateMap<Distribuir, DistribuirDTO>().ReverseMap();
        }
    }
}
