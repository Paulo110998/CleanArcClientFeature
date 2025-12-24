using MediatR;
using CleanArcClientFeature.Domain.Entities;

namespace CleanArcClientFeature.Application.Clients.Commands;

public abstract class ClientCommand : IRequest<Client>
{
    public string? NomeFantasia { get; set; }
    public string? Cnpj { get; set; } // Mantém como string
    public bool Ativo { get; set; }
}