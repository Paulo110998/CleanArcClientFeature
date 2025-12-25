using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class AtualizarClienteCommandHandler : IRequestHandler<AtualizarClienteCommand, Cliente>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapeador;

    public AtualizarClienteCommandHandler(IClienteRepository clienteRepository, IMapper mapeador)
    {
        _clienteRepository = clienteRepository;
        _mapeador = mapeador;
    }

    public async Task<Cliente> Handle(AtualizarClienteCommand requisicao, CancellationToken cancellationToken)
    {
        var ClienteExistente = await _clienteRepository.BuscarClienteIdAsync(requisicao.Id);
        if (ClienteExistente == null)
            throw new Exception("Cliente não encontrado");

        _mapeador.Map(requisicao, ClienteExistente);
        return await _clienteRepository.Atualizar(ClienteExistente);
    }
}