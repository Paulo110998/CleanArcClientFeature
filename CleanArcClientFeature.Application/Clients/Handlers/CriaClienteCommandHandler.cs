using AutoMapper;
using CleanArcClientFeature.Application.Clients.Commands;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Domain.ValueObjects;
using CleanArcClientFeature.Infrastructure.Interfaces;
using MediatR;
using System.Text.RegularExpressions;


namespace CleanArcClientFeature.Application.Clients.Handlers;



public class CriaClienteCommandHandler : IRequestHandler<CriaClienteCommand, Cliente>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapeador;

    public CriaClienteCommandHandler(IClienteRepository clienteRepository, IMapper mapeador)
    {
        _clienteRepository = clienteRepository;
        _mapeador = mapeador;
    }

    public async Task<Cliente> Handle(CriaClienteCommand requisicao, CancellationToken cancellationToken)
    {
        // Primeiro normaliza o CNPJ
        string cnpjNormalizado = NormalizarCnpj(requisicao.Cnpj);

        // Valida se CNPJ já existe (com CNPJ normalizado)
        var clienteExistente = await _clienteRepository.BuscarClientePorCnpjAsync(cnpjNormalizado);
        if (clienteExistente != null)
        {
            throw new InvalidOperationException($"Já existe um cliente cadastrado com o CNPJ {requisicao.Cnpj}");
        }

        // VALIDAÇÃO COMPLETA DO CNPJ (usa o normalizado)
        ValidarCnpj(cnpjNormalizado);

        // Cria o Value Object Cnpj
        var cnpj = new Cnpj(cnpjNormalizado);

        // Cria a entidade Client
        var cliente = new Cliente(requisicao.NomeFantasia, cnpj, requisicao.Ativo);

        return await _clienteRepository.Criar(cliente);
    }

    private string NormalizarCnpj(string cnpj)
    {
        // Remove todos os caracteres não numéricos
        return new string(cnpj?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
    }

    private void ValidarCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ não pode ser vazio");

        // Verifica se tem 14 dígitos
        if (cnpj.Length != 14)
            throw new ArgumentException("CNPJ deve ter 14 dígitos");

        // Verifica se contém apenas números
        if (!Regex.IsMatch(cnpj, @"^\d+$"))
            throw new ArgumentException("CNPJ deve conter apenas números");

        // Valida dígitos verificadores
        if (!ValidarDigitosVerificadores(cnpj))
            throw new ArgumentException("CNPJ inválido ou inexistente, adicione um cnpj real. Para testes, utilize: '00000000000191' ou '11222333000181'");
    }

    private bool ValidarDigitosVerificadores(string cnpj)
    {
        // Validação básica
        if (cnpj.Length != 14)
            return false;

        // 1. PRIMEIRO verifica se é CNPJ de teste
        string[] cnpjsTesteValidos = { "00000000000191", "11222333000181" };
        if (cnpjsTesteValidos.Contains(cnpj))
            return true;

        // 2. Para CNPJs não-teste, verifica repetição com regra mais branda
        // Apenas bloqueia se tiver 1 ou 2 dígitos diferentes
        if (cnpj.Distinct().Count() <= 2)
            return false;

        // Cálculo do primeiro dígito
        int[] pesos1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += (cnpj[i] - '0') * pesos1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        // Cálculo do segundo dígito
        int[] pesos2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += (cnpj[i] - '0') * pesos2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        // Verificação final
        return (cnpj[12] - '0') == digito1 && (cnpj[13] - '0') == digito2;
    }
}