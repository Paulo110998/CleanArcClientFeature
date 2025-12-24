using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;

namespace CleanArcClientFeature.Application.Clients.Handlers;

public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, Client>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public UpdateClienteCommandHandler(IClienteRepository clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<Client> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
        var existingClient = await _clienteRepository.GetClientIdAsync(request.Id);
        if (existingClient == null)
            throw new Exception("Cliente não encontrado");

        _mapper.Map(request, existingClient);
        return await _clienteRepository.Update(existingClient);
    }
}