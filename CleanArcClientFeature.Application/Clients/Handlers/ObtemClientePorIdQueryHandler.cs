using CleanArcClientFeature.Application.Clients.Queries;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class ObtemClientePorIdQueryHandler : IRequestHandler<ObtemClientePorIdQuery, Cliente>
{
    private readonly IClienteRepository _clienteRepository;

    public ObtemClientePorIdQueryHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Cliente> Handle(ObtemClientePorIdQuery requisicao, CancellationToken cancellationToken)
    {
        return await _clienteRepository.BuscarClienteIdAsync(requisicao.Id);
    }
}