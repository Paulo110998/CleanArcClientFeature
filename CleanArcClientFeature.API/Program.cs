using CleanArcClientFeature.Ioc;
using NHibernate;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

// Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/debug-assemblies", () =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.FullName != null && a.FullName.Contains("CleanArcClientFeature"))
        .Select(a => new
        {
            Name = a.GetName().Name,
            FullName = a.FullName,
            Location = a.Location,
            TypesCount = a.GetTypes().Length
        })
        .ToList();

    return Results.Ok(assemblies);
});

app.MapGet("/debug-nhibernate-internal", (IServiceProvider services) =>
{
    try
    {
        var factory = services.GetRequiredService<ISessionFactory>();
        var metadata = factory.GetClassMetadata(typeof(CleanArcClientFeature.Domain.Entities.Client));

        return Results.Ok(new
        {
            HasMetadata = metadata != null,
            MetadataType = metadata?.GetType().Name,
            EntityName = metadata?.EntityName,
            IdentifierPropertyName = metadata?.IdentifierPropertyName,
            PropertyNames = metadata?.PropertyNames
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Erro: {ex.Message}");
    }
});

// Endpoint corrigido para debug de mapeamentos
app.MapGet("/debug-mappings", (IServiceProvider services) =>
{
    try
    {
        var factory = services.GetRequiredService<ISessionFactory>();
        var allClassMetadata = factory.GetAllClassMetadata();

        var result = new List<object>();

        foreach (var entry in allClassMetadata)
        {
            var metadata = entry.Value as NHibernate.Persister.Entity.IEntityPersister;
            if (metadata != null)
            {
                result.Add(new
                {
                    EntityName = entry.Key,
                    // Substituição do RootTableName por MappedClass.Name
                    TableName = metadata.MappedClass?.Name,
                    Properties = metadata.PropertyNames,
                    IdPropertyName = metadata.IdentifierPropertyName
                });
            }
        }

        return Results.Ok(new
        {
            MappedEntities = result.Count,
            Entities = result
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Erro: {ex.Message}");
    }
});

app.MapPost("/test-persist", async (NHibernate.ISession session) =>
{
    try
    {
        // Testa se o mapeamento está funcionando
        var cnpj = new CleanArcClientFeature.Domain.ValueObjects.Cnpj("12345678901234");
        var client = new CleanArcClientFeature.Domain.Entities.Client("Teste Persist", cnpj, true);

        // Verifica se é uma entidade conhecida
        var factory = session.SessionFactory;
        var classMetadata = factory.GetClassMetadata(typeof(CleanArcClientFeature.Domain.Entities.Client));

        if (classMetadata == null)
        {
            return Results.Problem("? Entidade Client não está mapeada!");
        }

        await session.SaveAsync(client);
        await session.FlushAsync();

        return Results.Ok(new
        {
            Success = true,
            Id = client.Id,
            EntityName = classMetadata.EntityName,
            Message = "Persistência funcionando!"
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"? Erro: {ex.Message}\n{ex.GetType().Name}");
    }
});

app.Run();