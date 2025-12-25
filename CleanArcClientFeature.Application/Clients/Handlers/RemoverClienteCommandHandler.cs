using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class RemoverClienteCommandHandler : IRequestHandler<RemoverClienteCommand, Cliente>
{
    private readonly IClienteRepository _clienteRepository;

    public RemoverClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Cliente> Handle(RemoverClienteCommand requisicao, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.BuscarClienteIdAsync(requisicao.Id);
        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        return await _clienteRepository.Remover(cliente);
    }
}