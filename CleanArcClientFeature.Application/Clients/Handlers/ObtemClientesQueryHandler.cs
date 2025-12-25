using CleanArcClientFeature.Application.Clients.Queries;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class ObtemClientesQueryHandler : IRequestHandler<ObtemClientesQuery, IEnumerable<Cliente>>
{
    private readonly IClienteRepository _clienteRepository;

    public ObtemClientesQueryHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<IEnumerable<Cliente>> Handle(ObtemClientesQuery requisicao, CancellationToken cancellationToken)
    {
        return await _clienteRepository.BuscarClientesAsync();
    }
}