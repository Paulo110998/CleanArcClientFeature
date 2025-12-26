# CleanArcClientFeature

Este README contém uma breve explicação de como o projeto foi estruturado e de suas funcionalidades, conforme solicitado no desafio técnico.

## Funcionalidades sugeridas

- Endpoint **POST** e **GET/id** para criar e consultar um cliente existente pelo seu ID  
- Endpoints adicionais: **GET**, **PUT** e **DELETE**

## Estrutura do Projeto

- **SuaSolucao.Domain**: Onde residem as entidades e regras de negócio (ex.: Cliente).
- **SuaSolucao.Application**: Onde ficam os casos de uso (Commands, Queries e Handlers).
- **SuaSolucao.Infrastructure**: Onde está a implementação da persistência de dados.
- **SuaSolucao.API**: Projeto Web API responsável por expor os endpoints.
- **SuaSolucao.Tests**: Projeto de testes unitários.

Estrutura adicional:
- **SuaSolucao.Ioc**

---

## Estrutura Geral do Projeto

### **CleanArcClientFeature.Application**
- **Clients (Implementação de CQRS)**
  - **Commands**: Comandos para operações de escrita.
  - **Handlers**: Manipuladores responsáveis por processar comandos e queries.
  - **Queries**: Consultas para operações de leitura.
- **DTOs**: Objetos de transferência de dados.
- **Interfaces**: Declaração da interface `IClienteService`.
- **Mappings**: Mapeamentos com AutoMapper entre entidades, DTOs e commands.
- **Services**: Classes de serviço que encapsulam a lógica de aplicação.

---

### **CleanArcClientFeature.Domain**
- **Entities**: Representação do modelo de domínio `Cliente` e a EntidadeBase.
- **Validation**: Contém a classe `ExcecaoDeValidacaoDeDominio` para validações de domínio.
- **ValueObjects**: Contém a classe `Cnpj` (Value Object).

---

### **CleanArcClientFeature.Infrastructure**
- **Config**: Contém a classe `DatabaseConfig`, responsável por gerenciar as configurações do banco de dados em memória.
- **Helpers**: Contém a classe `DatabaseHelper`, com métodos para criação de conexão, criação de tabelas e inicialização do banco em memória.
- **Interfaces**: Declaração das interfaces `IClienteRepository` e `IDatabaseConfig`.
- **Mappings**: Mapeamento da entidade `Cliente` para a tabela `Clients`, definindo chave primária e propriedades no NHibernate.
- **Repositories**: Contém a classe `ClienteRepository`, responsável pela persistência de dados utilizando NHibernate.
- **Types**: Contém a classe `CnpjTipo`, utilizada para auxiliar nas validações do tipo CNPJ.

---

### **CleanArcClientFeature.Ioc (Camada adicional)**
- **InjecaoDependencia**: Classe responsável pela inversão de controle e injeção de dependências.

---

### **CleanArcClientFeature.Tests**
- **CriaClienteCommandHandlerTests**: Classe de testes unitários que valida o Handler de criação nos cenários propostos no desafio e em casos adicionais, utilizando xUnit e Fluent Assertions.
- **ObtemClientePorIdQueryHandlerTests**: Classe de testes unitários que valida o Handler de busca por ID nos cenários propostos no desafio e em casos adicionais, utilizando xUnit e Fluent Assertions.

---

### **CleanArcClientFeature.API**
- **Controllers**
  - **ClientesController**: Possui os endpoints **POST**, **GET/id**, **GET** (adicional), **PUT/id** (adicional) e **DELETE/id**.
- **Program.cs**: Contém as configurações básicas da API.
