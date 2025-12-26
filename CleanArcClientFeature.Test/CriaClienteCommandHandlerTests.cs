using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Application.Clients.Handlers;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;
using CleanArcClientFeature.Domain.Validation;
using CleanArcClientFeature.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using AutoMapper;

namespace CleanArcClientFeature.Test.Application.Handlers;

public class CriaClienteCommandHandlerTests
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CriaClienteCommandHandler _handler;

    public CriaClienteCommandHandlerTests()
    {   
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CriaClienteCommandHandler(_clienteRepositoryMock.Object, _mapperMock.Object);
    }

    // Teste 1: Deve criar um cliente com sucesso quando os dados são válidos
    [Fact(DisplayName = "CriaClienteCommandHandler - Dados válidos - Criar cliente com sucesso")]
    public async Task Handle_ComDadosValidos_DeveCriarCliente()
    {
        // Arrange
        var command = new CriaClienteCommand
        {
            NomeFantasia = "Empresa Teste LTDA",
            Cnpj = "12345678000195", // CNPJ válido (14 dígitos)
            Ativo = true
        };

        var clienteEsperado = new Cliente(
            1,
            command.NomeFantasia,
            new Cnpj(command.Cnpj),
            command.Ativo
        );

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClientePorCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((Cliente)null);

        _clienteRepositoryMock
            .Setup(repo => repo.Criar(It.IsAny<Cliente>()))
            .ReturnsAsync(clienteEsperado);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.NomeFantasia.Should().Be(command.NomeFantasia);
        resultado.Cnpj.Value.Should().Be("12345678000195"); // Valor formatado/limpo
        resultado.Ativo.Should().BeTrue();

        _clienteRepositoryMock.Verify(repo =>
            repo.Criar(It.IsAny<Cliente>()), Times.Once);
    }

    // Teste 2: Deve retornar um erro se o CNPJ já existir
    [Fact(DisplayName = "CriaClienteCommandHandler - CNPJ existente - Deve lançar exceção")]
    public async Task Handle_CnpjExistente_DeveLancarExcecao()
    {
        // Arrange
        var command = new CriaClienteCommand
        {
            NomeFantasia = "Empresa Teste LTDA",
            Cnpj = "12345678000195",
            Ativo = true
        };

        var clienteExistente = new Cliente(
            1,
            "Empresa Existente",
            new Cnpj(command.Cnpj),
            true
        );

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClientePorCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync(clienteExistente);

        // Act & Assert
        // Mude de Exception para InvalidOperationException
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain($"Já existe um cliente cadastrado com o CNPJ {command.Cnpj}");

        _clienteRepositoryMock.Verify(repo =>
            repo.Criar(It.IsAny<Cliente>()), Times.Never);
    }

    // Teste 3: Deve retornar um erro se dados essenciais forem inválidos (nome vazio)
    [Fact(DisplayName = "CriaClienteCommandHandler - Nome vazio - Deve lançar exceção de domínio")]
    public async Task Handle_NomeVazio_DeveLancarExcecaoDominio()
    {
        // Arrange
        var command = new CriaClienteCommand
        {
            NomeFantasia = "", // Nome vazio
            Cnpj = "12345678000195",
            Ativo = true
        };

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClientePorCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((Cliente)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExcecaoDeValidacaoDeDominio>(() =>
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Invalid Nome Fantasia");

        _clienteRepositoryMock.Verify(repo =>
            repo.Criar(It.IsAny<Cliente>()), Times.Never);
    }

    // Teste 4: Deve retornar um erro se CNPJ for nulo
    [Fact(DisplayName = "CriaClienteCommandHandler - CNPJ nulo - Deve lançar ArgumentException")]
    public async Task Handle_CnpjNulo_DeveLancarArgumentException()
    {
        // Arrange
        var command = new CriaClienteCommand
        {
            NomeFantasia = "Empresa Teste LTDA",
            Cnpj = null, // CNPJ nulo
            Ativo = true
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("CNPJ não pode ser vazio");

        _clienteRepositoryMock.Verify(repo =>
            repo.Criar(It.IsAny<Cliente>()), Times.Never);
    }

    // Teste 5: CNPJ com formato inválido (todos dígitos iguais)
    [Fact(DisplayName = "CriaClienteCommandHandler - CNPJ todos dígitos iguais - Deve lançar ArgumentException")]
    public async Task Handle_CnpjTodosDigitosIguais_DeveLancarArgumentException()
    {
        // Arrange
        var command = new CriaClienteCommand
        {
            NomeFantasia = "Empresa Teste LTDA",
            Cnpj = "00000000000000", // Todos dígitos iguais
            Ativo = true
        };

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClientePorCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((Cliente)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("CNPJ inválido ou inexistente, adicione um cnpj real. Para testes, utilize: '00000000000191' ou '11222333000181'");

        _clienteRepositoryMock.Verify(repo =>
            repo.Criar(It.IsAny<Cliente>()), Times.Never);
    }

    // Teste 6: CNPJ com dígitos verificadores inválidos
    [Fact(DisplayName = "CriaClienteCommandHandler - CNPJ dígitos verificadores inválidos - Deve lançar ArgumentException")]
    public async Task Handle_CnpjDigitosVerificadoresInvalidos_DeveLancarArgumentException()
    {
        // Arrange
        var command = new CriaClienteCommand
        {
            NomeFantasia = "Empresa Teste LTDA",
            Cnpj = "11222333000182", // Alterado para realmente inválido
            Ativo = true
        };

        _clienteRepositoryMock
            .Setup(repo => repo.BuscarClientePorCnpjAsync(It.IsAny<string>()))
            .ReturnsAsync((Cliente)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        // Mensagem
        exception.Message.Should().Contain("CNPJ inválido ou inexistente, adicione um cnpj real. Para testes, utilize: '00000000000191' ou '11222333000181'");


        _clienteRepositoryMock.Verify(repo =>
            repo.Criar(It.IsAny<Cliente>()), Times.Never);
    }
}