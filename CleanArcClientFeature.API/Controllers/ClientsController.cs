using CleanArcClientFeature.Application.DTOs;
using CleanArcClientFeature.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcClientFeature.API.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IWebHostEnvironment _enviroment;

    public ClientsController(IClientService clientService, IWebHostEnvironment enviroment)
    {
        _clientService = clientService;
        _enviroment = enviroment;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] ClientDTO clientDTO)
    {
        await _clientService.Add(clientDTO);
        return Ok("Cliente criado com sucesso.");  
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var clients = await _clientService.GetClients();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var client = await _clientService.GetClientById(id);
        if (client == null)
        {
            return NotFound("Cliente não encontrado.");
        }

        return Ok(client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDTO clientDTO)
    {
        var existingClient = await _clientService.GetClientById(id);
        if (existingClient == null)
        {
            return NotFound("Cliente não encontrado.");
        }
        await _clientService.Update(clientDTO);
        return Ok("Cliente atualizado com sucesso.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var existingClient = await _clientService.GetClientById(id);
        
        if (existingClient == null)
        {
            return NotFound("Cliente não encontrado.");
        }
        
        await _clientService.Remove(id);
        return Ok("Cliente removido com sucesso.");
    }

}
