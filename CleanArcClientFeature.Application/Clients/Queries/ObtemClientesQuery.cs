using CleanArcClientFeature.Domain.Entities;
using MediatR;


namespace CleanArcClientFeature.Application.Clients.Queries;

public class ObtemClientesQuery : IRequest<IEnumerable<Cliente>>
{
}
