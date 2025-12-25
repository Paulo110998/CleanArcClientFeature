using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.DTOs;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;

namespace CleanArcClientFeature.Application.Mappings;

public class ProfileDeMapeamentoDeDominioParaDTO : Profile
{
    public ProfileDeMapeamentoDeDominioParaDTO()
    {
        // Mapeamento de Client para ClientDTO
        CreateMap<Cliente, ClienteDTO>()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
            .ReverseMap()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new Cnpj(src.Cnpj)));

        // Mapeamento para Commands
        CreateMap<Cliente, CriaClienteCommand>()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
            .ReverseMap()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new Cnpj(src.Cnpj)));

        CreateMap<Cliente, AtualizarClienteCommand>()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
            .ReverseMap()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new Cnpj(src.Cnpj)));
    }
}