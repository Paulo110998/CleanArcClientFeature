using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class RemoveClienteCommandHandler : IRequestHandler<RemoveClienteCommand, Client>
{
    private readonly IClienteRepository _clienteRepository;

    public RemoveClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Client> Handle(RemoveClienteCommand request, CancellationToken cancellationToken)
    {
        var client = await _clienteRepository.GetClientIdAsync(request.Id);
        if (client == null)
            throw new Exception("Cliente não encontrado");

        return await _clienteRepository.Remove(client);
    }
}