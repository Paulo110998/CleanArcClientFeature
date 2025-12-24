using CleanArcClientFeature.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArcClientFeature.Application.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientDTO>> GetClients();

    Task<ClientDTO> GetClientById(int? id);

    Task Add(ClientDTO clientDTO);

    Task Update(ClientDTO clientDTO);

    Task Remove(int? id);
}
