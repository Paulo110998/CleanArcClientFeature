using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class CriaClienteCommandHandler : IRequestHandler<CriaClienteCommand, Cliente>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapeador;

    public CriaClienteCommandHandler(IClienteRepository clienteRepository, IMapper mapeador)
    {
        _clienteRepository = clienteRepository;
        _mapeador = mapeador;
    }

    public async Task<Cliente> Handle(CriaClienteCommand requisicao, CancellationToken cancellationToken)
    {
        // Cria o Value Object Cnpj
        var cnpj = new Cnpj(requisicao.Cnpj);

        // Cria a entidade Client
        var cliente = new Cliente(requisicao.NomeFantasia, cnpj, requisicao.Ativo);

        return await _clienteRepository.Criar(cliente);
    }
}