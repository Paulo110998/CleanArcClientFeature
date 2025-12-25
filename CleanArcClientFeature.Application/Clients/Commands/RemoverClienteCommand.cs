using CleanArcClientFeature.Domain.Entities;
using MediatR;


namespace CleanArcClientFeature.Application.Clients.Commands;

public class RemoverClienteCommand : IRequest<Cliente>
{
    public int Id { get; set; }

    public RemoverClienteCommand(int id)
    {
        Id = id;        
    }
}
