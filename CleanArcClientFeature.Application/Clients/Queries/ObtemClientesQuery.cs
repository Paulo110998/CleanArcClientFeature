using CleanArcClientFeature.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Application.Clients.Queries;

public class ObtemClientesQuery : IRequest<IEnumerable<Client>>
{
}
