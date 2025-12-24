using CleanArcClientFeature.Application.Interfaces;
using CleanArcClientFeature.Application.Mappings;
using CleanArcClientFeature.Application.Services;
using CleanArcClientFeature.Domain.Entities;
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
using System.Reflection;

namespace CleanArcClientFeature.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISessionFactory>(sp =>
        {
            try
            {
                var cfg = new Configuration();

                // Configuração SIMPLIFICADA com System.Data.SQLite
                cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                    db.Dialect<SQLiteDialect>();
                    db.Driver<SQLite20Driver>();
                    db.SchemaAction = SchemaAutoAction.Create;
                    db.LogSqlInConsole = true;
                    db.LogFormattedSql = true;
                });

                // DESABILITA validação de schema que causa o erro
                cfg.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");
                cfg.SetProperty(NHibernate.Cfg.Environment.UseSqlComments, "false");

                // **CORREÇÃO: Carregar assembly do Infrastructure corretamente**
                var mapper = new ModelMapper();

                // Use o tipo ClientMap para obter a assembly correta
                var infrastructureAssembly = typeof(ClientMap).Assembly;
                Console.WriteLine($"🔍 Carregando assembly: {infrastructureAssembly.FullName}");

                // Busca TODOS os tipos de mapeamento (não só por namespace)
                var mappingTypes = infrastructureAssembly.GetExportedTypes()
                    .Where(t => typeof(IClassMapper).IsAssignableFrom(t) && !t.IsAbstract)
                    .ToList();

                Console.WriteLine($"✅ Encontrados {mappingTypes.Count} mapeamentos:");
                foreach (var type in mappingTypes)
                {
                    Console.WriteLine($"   - {type.FullName}");
                    mapper.AddMapping(type);
                }

                // Se não encontrou, tenta carregar por namespace específico
                if (!mappingTypes.Any())
                {
                    Console.WriteLine("⚠️  Tentando carregar por namespace...");
                    var namespaceTypes = infrastructureAssembly.GetExportedTypes()
                        .Where(t => t.Namespace != null &&
                                  t.Namespace.Contains("Mappings") &&
                                  !t.IsAbstract);

                    foreach (var type in namespaceTypes)
                    {
                        Console.WriteLine($"   - {type.FullName}");
                        mapper.AddMapping(type);
                    }
                }

                var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                cfg.AddMapping(mapping);

                Console.WriteLine($"✅ Mapeamento compilado com sucesso!");

                var sessionFactory = cfg.BuildSessionFactory();
                Console.WriteLine("✅ SessionFactory criado com sucesso!");
                return sessionFactory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao criar SessionFactory: {ex.Message}");
                Console.WriteLine($"📋 Tipo: {ex.GetType().FullName}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                    Console.WriteLine($"📋 Inner Tipo: {ex.InnerException.GetType().FullName}");
                }
                throw;
            }
        });

        services.AddScoped<ISession>(sp =>
        {
            var sessionFactory = sp.GetRequiredService<ISessionFactory>();
            var session = sessionFactory.OpenSession();
            InitializeDatabase(session);
            return session;
        });

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClientService, ClientService>();
        services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.Load("CleanArcClientFeature.Application"));
        });

        return services;
    }

    private static void InitializeDatabase(ISession session)
    {
        using var transaction = session.BeginTransaction();
        try
        {
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NomeFantasia TEXT NOT NULL,
                    Cnpj TEXT NOT NULL,
                    Ativo BOOLEAN NOT NULL
                )";

            session.CreateSQLQuery(createTableSql).ExecuteUpdate();
            transaction.Commit();
            Console.WriteLine("✅ Tabela 'Clients' criada/verificada.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao criar tabela: {ex.Message}");
            transaction.Rollback();
            throw;
        }
    }
}