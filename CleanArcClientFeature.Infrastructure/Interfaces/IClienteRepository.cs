using CleanArcClientFeature.Domain.Entities;

namespace CleanArcClientFeature.Infrastructure.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> BuscarClientesAsync();

    Task<Cliente> BuscarClienteIdAsync(int? id);

    Task<Cliente> Criar(Cliente client);

    Task<Cliente> Atualizar(Cliente client);

    Task<Cliente> Remover(Cliente client);
}
