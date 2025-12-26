using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace CleanArcClientFeature.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly List<Cliente> _clientes = new();
    private readonly ISession _session;

    public ClienteRepository(ISession session)
    {
        _session = session;
    }

    // CREATE - Versão simplificada
    public async Task<Cliente> Criar(Cliente client)
    {
        // O NHibernate gerencia a transação automaticamente em alguns casos,
        // mas vamos manter explícito para clareza
        using var transaction = _session.BeginTransaction();
        try
        {
            await _session.SaveAsync(client); // erro aqui
            await transaction.CommitAsync();
            return client;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Cliente?> BuscarClienteIdAsync(int? id)
    {
        if (id == null) return null;
        return await _session.GetAsync<Cliente>(id.Value);
    }

    public async Task<IEnumerable<Cliente>> BuscarClientesAsync()
    {
        return await _session.Query<Cliente>().ToListAsync();
    }

    public async Task<Cliente> Remover(Cliente client)
    {
        using var transaction = _session.BeginTransaction();
        try
        {
            await _session.DeleteAsync(client);
            await transaction.CommitAsync();
            return client;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Cliente> Atualizar(Cliente client)
    {
        using var transaction = _session.BeginTransaction();
        try
        {
            await _session.UpdateAsync(client);
            await transaction.CommitAsync();
            return client;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Cliente> BuscarClientePorCnpjAsync(string cnpj)
    {
        // Normaliza o CNPJ antes de buscar
        string cnpjNormalizado = new string(cnpj?.Where(char.IsDigit).ToArray());

        // Consulta alternativa que o NHibernate consegue processar
        var clientes = await _session.Query<Cliente>().ToListAsync();

        return clientes.FirstOrDefault(c =>
            new string(c.Cnpj.Value?.Where(char.IsDigit).ToArray()) == cnpjNormalizado);
    }

  
}