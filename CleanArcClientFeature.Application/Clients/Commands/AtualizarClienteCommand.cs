using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Application.Clients.Commands;

public class AtualizarClienteCommand : ClientCommand
{
    public int Id { get; set; }
}
