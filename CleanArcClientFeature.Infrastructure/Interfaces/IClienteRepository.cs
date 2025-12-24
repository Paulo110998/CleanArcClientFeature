using CleanArcClientFeature.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Infrastructure.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<Client>> GetClientsAsync();

    Task<Client> GetClientIdAsync(int? id);

    Task<Client> Create(Client client);

    Task<Client> Update(Client client);

    Task<Client> Remove(Client client);
}
