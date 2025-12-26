using CleanArcClientFeature.Application.DTOs;
using CleanArcClientFeature.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcClientFeature.API.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly IWebHostEnvironment _enviroment;

    public ClientesController(IClienteService clientService, IWebHostEnvironment enviroment)
    {
        _clienteService = clientService;
        _enviroment = enviroment;
    }

    [HttpPost]
    public async Task<IActionResult> CriarClient([FromBody] ClienteDTO clientDTO)
    {
        try
        {
            await _clienteService.Adicionar(clientDTO);
            return Ok("Cliente criado com sucesso.");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet]
    public async Task<IActionResult> BuscarClients()
    {
        var clients = await _clienteService.BuscarClientes();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarClienteId(int id)
    {
        var client = await _clienteService.BuscarClienteById(id);
        if (client == null)
        {
            return NotFound("Cliente não encontrado.");
        }

        return Ok(client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCliente(int id, [FromBody] ClienteDTO clientDTO)
    {
        var existingClient = await _clienteService.BuscarClienteById(id);
        if (existingClient == null)
        {
            return NotFound("Cliente não encontrado.");
        }
        await _clienteService.Atualizar(clientDTO);
        return Ok("Cliente atualizado com sucesso.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCliente(int id)
    {
        var existingClient = await _clienteService.BuscarClienteById(id);
        
        if (existingClient == null)
        {
            return NotFound("Cliente não encontrado.");
        }
        
        await _clienteService.Remover(id);
        return Ok("Cliente removido com sucesso.");
    }

}
