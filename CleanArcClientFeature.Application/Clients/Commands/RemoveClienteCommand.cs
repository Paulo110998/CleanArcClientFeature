using CleanArcClientFeature.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Application.Clients.Commands;

public class RemoveClienteCommand : IRequest<Client>
{
    public int Id { get; set; }

    public RemoveClienteCommand(int id)
    {
        Id = id;        
    }
}
