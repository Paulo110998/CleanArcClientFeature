using CleanArcClientFeature.Application.Clients.Queries;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class ObtemClientesQueryHandler : IRequestHandler<ObtemClientesQuery, IEnumerable<Client>>
{
    private readonly IClienteRepository _clienteRepository;

    public ObtemClientesQueryHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<IEnumerable<Client>> Handle(ObtemClientesQuery request, CancellationToken cancellationToken)
    {
        return await _clienteRepository.GetClientsAsync();
    }
}