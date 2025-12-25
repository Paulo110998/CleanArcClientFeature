using CleanArcClientFeature.Application.DTOs;

namespace CleanArcClientFeature.Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDTO>> BuscarClientes();

    Task<ClienteDTO> BuscarClienteById(int? id);

    Task Adicionar(ClienteDTO clientDTO);

    Task Atualizar(ClienteDTO clientDTO);

    Task Remover(int? id);
}
