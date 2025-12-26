using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.Clients.Queries;
using CleanArcClientFeature.Application.DTOs;
using CleanArcClientFeature.Application.Interfaces;
using CleanArcClientFeature.Domain.Entities;
using MediatR;

namespace CleanArcClientFeature.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ClienteService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IEnumerable<ClienteDTO>> BuscarClientes()
    {
        var query = new ObtemClientesQuery();
        var clients = await _mediator.Send(query);
        return _mapper.Map<IEnumerable<ClienteDTO>>(clients);
    }

    public async Task<ClienteDTO> BuscarClienteById(int? id)
    {
        if (id == null) return null;

        var query = new ObtemClientePorIdQuery(id.Value);
        var client = await _mediator.Send(query);
        return _mapper.Map<ClienteDTO>(client);
    }

  
    public async Task Adicionar(ClienteDTO clientDTO)
    {
        try
        {
            var command = _mapper.Map<CriaClienteCommand>(clientDTO);
            await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            // A exceção lançada no Handle será capturada aqui
            // Você pode logar se quiser e depois relançar para mostrar ao usuário
            throw new Exception($"Erro ao adicionar cliente: {ex.Message}");
        }
    }

    public async Task Atualizar(ClienteDTO clientDTO)
    {
        var command = _mapper.Map<AtualizarClienteCommand>(clientDTO);
        await _mediator.Send(command);
    }

    public async Task Remover(int? id)
    {
        if (id == null) return;

        var command = new RemoverClienteCommand(id.Value);
        await _mediator.Send(command);
    }
}