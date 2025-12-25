using CleanArcClientFeature.Application.Interfaces;
using CleanArcClientFeature.Application.Mappings;
using CleanArcClientFeature.Application.Services;
using CleanArcClientFeature.Infrastructure.Interfaces;
using CleanArcClientFeature.Infrastructure.Mappings;
using CleanArcClientFeature.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

namespace CleanArcClientFeature.Ioc;

public static class InjecaoDependencia
{
    private static SQLiteConnection? _conexaoCompartilhada;

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Cria uma ÚNICA conexão compartilhada
        _conexaoCompartilhada = CriarConexaoCompartilhada();

        services.AddSingleton<ISessionFactory>(sp =>
        {
            try
            {
                var cfg = new Configuration();

                // Usar a conexão compartilhada
                cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionString = _conexaoCompartilhada.ConnectionString;
                    db.Dialect<SQLiteDialect>();
                    db.Driver<SQLite20Driver>();
                    db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                    db.LogSqlInConsole = true;
                    db.LogFormattedSql = true;
                });

                // Forçar criação de schema
                cfg.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "update");
                cfg.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");

                // Carrega mapeamentos
                var mapeador = new ModelMapper();
                mapeador.AddMapping<ClienteMap>();

                // Registra o CnpjType
                cfg.AddAuxiliaryDatabaseObject(new RegistroTipoCnpj());

                var mapeamento = mapeador.CompileMappingForAllExplicitlyAddedEntities();
                cfg.AddMapping(mapeamento);

                // Cria SessionFactory com conexão compartilhada
                var sessionFactory = cfg.BuildSessionFactory();

                // Cria tabelas na conexão compartilhada
                CriarTabelasSeNaoExistirem();

                Console.WriteLine("SessionFactory criado com sucesso!");
                return sessionFactory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar SessionFactory: {ex.Message}");
                throw;
            }
        });

        //  Usa a mesma conexão compartilhada para todas as sessões
        services.AddScoped<NHibernate.ISession>(sp =>
        {
            var factory = sp.GetRequiredService<ISessionFactory>();
            // Abre uma sessão usando a conexão compartilhada
            return factory.WithOptions()
                .Connection(_conexaoCompartilhada)
                .OpenSession();
        });

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddAutoMapper(typeof(ProfileDeMapeamentoDeDominioParaDTO));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.Load("CleanArcClientFeature.Application"));
        });

        return services;
    }

    private static SQLiteConnection CriarConexaoCompartilhada()
    {
        // Usa arquivo físico para desenvolvimento (mais estável)
        var dbCaminho = Path.Combine(AppContext.BaseDirectory, "CleanArcClientFeature.db");
        var conexaoString = $"Data Source={dbCaminho};Version=3;";

        Console.WriteLine($" Banco de dados: {dbCaminho}");

        var conexao = new SQLiteConnection(conexaoString);
        conexao.Open();

        // Criar tabela imediatamente
        using (var cmd = new SQLiteCommand(
            @"CREATE TABLE IF NOT EXISTS Clients (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                NomeFantasia TEXT NOT NULL,
                Cnpj TEXT NOT NULL,
                Ativo BOOLEAN NOT NULL
            )", conexao))
        {
            cmd.ExecuteNonQuery();
        }

        Console.WriteLine("Tabela 'Clients' criada/verificada na conexão compartilhada.");
        return conexao;
    }

    private static void CriarTabelasSeNaoExistirem()
    {
        try
        {
            using (var cmd = new SQLiteCommand(
                @"CREATE TABLE IF NOT EXISTS Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NomeFantasia TEXT NOT NULL,
                    Cnpj TEXT NOT NULL,
                    Ativo BOOLEAN NOT NULL
                )", _conexaoCompartilhada))
            {
                cmd.ExecuteNonQuery();
            }

            // Verificar se a tabela existe
            using (var cmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Clients'",
                 _conexaoCompartilhada))
            {
                var result = cmd.ExecuteScalar();
                Console.WriteLine($"Tabela 'Clients' verificada: {(long)result == 1}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar tabelas: {ex.Message}");
        }
    }

    // Classe auxiliar para registrar o tipo customizado
    private class RegistroTipoCnpj : NHibernate.Mapping.IAuxiliaryDatabaseObject 
    {
        public void AddDialectScope(string dialectName) { }

        public bool AppliesToDialect(Dialect dialect)
        {
            throw new NotImplementedException();
        }

        public void SetParameterValues(IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        public string SqlCreateString(NHibernate.Dialect.Dialect dialect,
            NHibernate.Engine.IMapping p, string defaultCatalog, string defaultSchema)
            => string.Empty;

        public string SqlDropString(NHibernate.Dialect.Dialect dialect,
            string defaultCatalog, string defaultSchema)
            => string.Empty;
    }
}