using CleanArcClientFeature.Application.Interfaces;
using CleanArcClientFeature.Application.Mappings;
using CleanArcClientFeature.Application.Services;
using CleanArcClientFeature.Infrastructure.Config;
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
using System.Data.SQLite;
using System.Reflection;

namespace CleanArcClientFeature.Ioc;

public static class InjecaoDependencia
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrar a configuração do banco de dados
        services.AddSingleton<IDatabaseConfig>(sp =>
        {
            var databaseSection = configuration.GetSection("Database");
            var databaseName = databaseSection.GetChildren()
                .FirstOrDefault(s => s.Key == "Name")?.Value ?? "CleanArcClientFeature.db";
            return new DatabaseConfig(databaseName);
        });

        // Obter a conexão compartilhada via IDatabaseConfig
        services.AddSingleton(sp =>
        {
            var databaseConfig = sp.GetRequiredService<IDatabaseConfig>();
            return databaseConfig.CriarConexaoCompartilhada();
        });

        services.AddSingleton<ISessionFactory>(sp =>
        {
            try
            {
                var conexao = sp.GetRequiredService<SQLiteConnection>();
                var cfg = new Configuration();

                // Configurar NHibernate com a conexão compartilhada
                cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionString = conexao.ConnectionString;
                    db.Dialect<SQLiteDialect>();
                    db.Driver<SQLite20Driver>();
                    db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                    var logSqlSection = configuration.GetSection("Database").GetChildren()
                        .FirstOrDefault(s => s.Key == "LogSql");
                    db.LogSqlInConsole = logSqlSection != null && bool.TryParse(logSqlSection.Value, out var logSql) ? logSql : true;
                    db.LogFormattedSql = true;
                });

                // Configuração do schema
                cfg.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "update");
                cfg.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");

                // Carregar mapeamentos
                var mapeador = new ModelMapper();
                mapeador.AddMapping<ClienteMap>();
                var mapeamento = mapeador.CompileMappingForAllExplicitlyAddedEntities();
                cfg.AddMapping(mapeamento);

                var sessionFactory = cfg.BuildSessionFactory();

                // Criar tabelas se não existirem
                var databaseConfig = sp.GetRequiredService<IDatabaseConfig>();
                databaseConfig.CriarTabelasSeNaoExistirem();

                Console.WriteLine("SessionFactory criado com sucesso!");
                return sessionFactory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar SessionFactory: {ex.Message}");
                throw;
            }
        });

        // Registrar sessão do NHibernate
        services.AddScoped<NHibernate.ISession>(sp =>
        {
            var factory = sp.GetRequiredService<ISessionFactory>();
            var conexao = sp.GetRequiredService<SQLiteConnection>();

            return factory.WithOptions()
                .Connection(conexao)
                .OpenSession();
        });

        // Registrar repositórios e serviços
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClienteService, ClienteService>();

        // AutoMapper
        services.AddAutoMapper(typeof(ProfileDeMapeamentoDeDominioParaDTO));

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.Load("CleanArcClientFeature.Application"));
        });

        return services;
    }

    // Classe auxiliar para registro do tipo customizado 
    private class RegistroTipoCnpj : NHibernate.Mapping.IAuxiliaryDatabaseObject
    {
        public void AddDialectScope(string dialectName) { }

        public bool AppliesToDialect(Dialect dialect) => true;

        public void SetParameterValues(IDictionary<string, string> parameters) { }

        public string SqlCreateString(NHibernate.Dialect.Dialect dialect,
            NHibernate.Engine.IMapping p, string defaultCatalog, string defaultSchema)
            => string.Empty;

        public string SqlDropString(NHibernate.Dialect.Dialect dialect,
            string defaultCatalog, string defaultSchema)
            => string.Empty;
    }
}