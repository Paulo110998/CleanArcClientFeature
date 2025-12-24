using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Application.Mappings;

public class DtoToCommandMappingProfile : Profile
{
    public DtoToCommandMappingProfile()
    {
        CreateMap<ClientDTO, CriaClienteCommand>();
        CreateMap<ClientDTO, UpdateClienteCommand>();
    }
}
