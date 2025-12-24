using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class CriaClienteCommandHandler : IRequestHandler<CriaClienteCommand, Client>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public CriaClienteCommandHandler(IClienteRepository clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<Client> Handle(CriaClienteCommand request, CancellationToken cancellationToken)
    {
        // Cria o Value Object Cnpj
        var cnpj = new Cnpj(request.Cnpj);

        // Cria a entidade Client
        var client = new Client(request.NomeFantasia, cnpj, request.Ativo);

        return await _clienteRepository.Create(client);
    }
}