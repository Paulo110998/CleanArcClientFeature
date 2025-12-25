using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.DTOs;

namespace CleanArcClientFeature.Application.Mappings;

public class ProfileDeMapeamentoDeDTOParaCommand : Profile
{
    public ProfileDeMapeamentoDeDTOParaCommand()
    {
        CreateMap<ClienteDTO, CriaClienteCommand>();
        CreateMap<ClienteDTO, AtualizarClienteCommand>();
    }
}
