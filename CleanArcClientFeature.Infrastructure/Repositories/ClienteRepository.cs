using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace CleanArcClientFeature.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ISession _session;

    public ClienteRepository(ISession session)
    {
        _session = session;
    }

    // CREATE
    public async Task<Client> Create(Client client)
    {
        using (var transaction = _session.BeginTransaction())
        {
            await _session.SaveAsync(client); 
            await transaction.CommitAsync();
        }
        return client;
    }

    public async Task<Client?> GetClientIdAsync(int? id)
    {
        if (id == null)
            return null;

        return await _session.GetAsync<Client>(id.Value);
    }

    // GET
    public async Task<IEnumerable<Client>> GetClientsAsync()
    {
        return await _session.Query<Client>().ToListAsync();
    }

    // DELETE
    public async Task<Client> Remove(Client client)
    {
        using (var transaction = _session.BeginTransaction())
        {
            await _session.DeleteAsync(client);
            await transaction.CommitAsync();
        }
        return client;
    }

    // UPDATE
    public async Task<Client> Update(Client client)
    {
        using (var transaction = _session.BeginTransaction())
        {
            await _session.UpdateAsync(client);
            await transaction.CommitAsync();
        }
        return client;
    }
}