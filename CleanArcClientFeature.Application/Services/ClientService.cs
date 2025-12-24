using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.Clients.Queries;
using CleanArcClientFeature.Application.DTOs;
using CleanArcClientFeature.Application.Interfaces;
using CleanArcClientFeature.Domain.Entities;
using MediatR;

namespace CleanArcClientFeature.Application.Services;

public class ClientService : IClientService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ClientService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IEnumerable<ClientDTO>> GetClients()
    {
        var query = new ObtemClientesQuery();
        var clients = await _mediator.Send(query);
        return _mapper.Map<IEnumerable<ClientDTO>>(clients);
    }

    public async Task<ClientDTO> GetClientById(int? id)
    {
        if (id == null) return null;

        var query = new ObtemClientePorIdQuery(id.Value);
        var client = await _mediator.Send(query);
        return _mapper.Map<ClientDTO>(client);
    }

    public async Task Add(ClientDTO clientDTO)
    {
        var command = _mapper.Map<CriaClienteCommand>(clientDTO);
        await _mediator.Send(command);
    }

    public async Task Update(ClientDTO clientDTO)
    {
        var command = _mapper.Map<UpdateClienteCommand>(clientDTO);
        await _mediator.Send(command);
    }

    public async Task Remove(int? id)
    {
        if (id == null) return;

        var command = new RemoveClienteCommand(id.Value);
        await _mediator.Send(command);
    }
}