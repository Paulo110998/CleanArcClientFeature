using CleanArcClientFeature.Application.Clients.Queries;
using CleanArcClientFeature.Application.Clients.Handlers;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;
using CleanArcClientFeature.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArcClientFeature.Test.Application.Handlers;

public class ObtemClientePorIdQueryHandlerTests
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly ObtemClientePorIdQueryHandler _handler;

    public ObtemClientePorIdQueryHandlerTests()
    {
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _handler = new ObtemClientePorIdQueryHandler(_clienteRepositoryMock.Object);
    }

    // Teste 1: Deve retornar o cliente correto quando o ID existe
    [Fact(DisplayName = "ObtemClientePorIdQueryHandler - ID existente - Deve retornar cliente")]
    public async Task Handle_IdExistente_DeveRetornarCliente()
    {
        // Arrange
        var clienteId = 1;
        var cnpjValido = "11444777000161"; // CNPJ VÁLIDO CONHECIDO 

        var clienteEsperado = new Cliente(
            clienteId,
            "Empresa Teste LTDA",
            new Cnpj(cnpjValido),
            true
        );

        var query = new ObtemClientePorIdQuery(clienteId);

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClienteIdAsync(clienteId))
            .ReturnsAsync(clienteEsperado);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(clienteId);
        resultado.NomeFantasia.Should().Be("Empresa Teste LTDA");
        resultado.Cnpj.Value.Should().Be(cnpjValido);

        _clienteRepositoryMock.Verify(repo =>
            repo.BuscarClienteIdAsync(clienteId), Times.Once);
    }

    // Teste 2: Deve retornar nulo quando o ID não existe
    [Fact(DisplayName = "ObtemClientePorIdQueryHandler - ID não existente - Deve retornar nulo")]
    public async Task Handle_IdNaoExistente_DeveRetornarNulo()
    {
        // Arrange
        var clienteId = 999; // ID que não existe
        var query = new ObtemClientePorIdQuery(clienteId);

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClienteIdAsync(clienteId))
            .ReturnsAsync((Cliente)null);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeNull();

        _clienteRepositoryMock.Verify(repo =>
            repo.BuscarClienteIdAsync(clienteId), Times.Once);
    }

    // Teste 3: Deve retornar nulo quando ID é zero
    [Fact(DisplayName = "ObtemClientePorIdQueryHandler - ID zero - Deve retornar nulo")]
    public async Task Handle_IdZero_DeveRetornarNulo()
    {
        // Arrange
        var query = new ObtemClientePorIdQuery(0);

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClienteIdAsync(0))
            .ReturnsAsync((Cliente)null);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeNull();
    }

    // Teste 4: Deve retornar nulo quando ID é negativo
    [Fact(DisplayName = "ObtemClientePorIdQueryHandler - ID negativo - Deve retornar nulo")]
    public async Task Handle_IdNegativo_DeveRetornarNulo()
    {
        // Arrange
        var query = new ObtemClientePorIdQuery(-1);

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClienteIdAsync(-1))
            .ReturnsAsync((Cliente)null);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeNull();
    }
}