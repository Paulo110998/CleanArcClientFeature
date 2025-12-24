using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.DTOs;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;

namespace CleanArcClientFeature.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        // Mapeamento de Client para ClientDTO
        CreateMap<Client, ClientDTO>()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
            .ReverseMap()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new Cnpj(src.Cnpj)));

        // Mapeamento para Commands
        CreateMap<Client, CriaClienteCommand>()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
            .ReverseMap()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new Cnpj(src.Cnpj)));

        CreateMap<Client, UpdateClienteCommand>()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.Value))
            .ReverseMap()
            .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => new Cnpj(src.Cnpj)));
    }
}