using CleanArcClientFeature.Domain.Entities;
using MediatR;


namespace CleanArcClientFeature.Application.Clients.Queries;

public class ObtemClientePorIdQuery : IRequest<Cliente>
{
    public int Id { get; set; }

    public ObtemClientePorIdQuery(int id)
    {
        Id = id;

    }
}
