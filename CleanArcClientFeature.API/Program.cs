using CleanArcClientFeature.Ioc;
using NHibernate;
using System.Data.SQLite;
using NHibernateSession = NHibernate.ISession;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

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

//// Endpoint para verificar o banco de dados
//app.MapGet("/debug-db-status", (NHibernateSession session, IServiceProvider services) =>
//{
//    try
//    {
//        var factory = services.GetRequiredService<ISessionFactory>();

//        // Verificar tabelas via SQL puro
//        var tables = session.CreateSQLQuery(
//            "SELECT name FROM sqlite_master WHERE type='table'")
//            .List<string>();

//        var clientsCount = tables.Contains("Clients")
//            ? session.CreateSQLQuery("SELECT COUNT(*) FROM Clients").UniqueResult<long>()
//            : 0;

//        // Verificar metadados do NHibernate
//        var metadata = factory.GetClassMetadata(typeof(CleanArcClientFeature.Domain.Entities.Client));

//        return Results.Ok(new
//        {
//            DatabaseFile = Path.Combine(AppContext.BaseDirectory, "CleanArcClientFeature.db"),
//            Tables = tables,
//            ClientsTableExists = tables.Contains("Clients"),
//            ClientsCount = clientsCount,
//            NHibernateMapped = metadata != null,
//            SessionFactoryType = factory.GetType().Name
//        });
//    }
//    catch (Exception ex)
//    {
//        return Results.Problem($"Erro: {ex.Message}");
//    }
//});

//// Endpoint para testar inserção SQL DIRETA (bypass NHibernate)
//app.MapPost("/test-sql-insert", () =>
//{
//    try
//    {
//        var dbPath = Path.Combine(AppContext.BaseDirectory, "CleanArcClientFeature.db");
//        using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
//        connection.Open();

//        using var cmd = new SQLiteCommand(
//            "INSERT INTO Clients (NomeFantasia, Cnpj, Ativo) VALUES (@nome, @cnpj, @ativo)",
//            connection);

//        cmd.Parameters.AddWithValue("@nome", "Teste SQL Direto");
//        cmd.Parameters.AddWithValue("@cnpj", "12345678901234");
//        cmd.Parameters.AddWithValue("@ativo", true);

//        var rowsAffected = cmd.ExecuteNonQuery();

//        // Obter o último ID inserido
//        cmd.CommandText = "SELECT last_insert_rowid()";
//        var lastId = cmd.ExecuteScalar();

//        return Results.Ok(new
//        {
//            Success = true,
//            RowsAffected = rowsAffected,
//            LastInsertId = lastId,
//            Message = "Inserção SQL direta funcionou!"
//        });
//    }
//    catch (Exception ex)
//    {
//        return Results.Problem($"❌ Erro SQL direto: {ex.Message}");
//    }
//});

//// Endpoint para limpar e recriar o banco (apenas para testes)
//app.MapPost("/reset-database", () =>
//{
//    try
//    {
//        var dbPath = Path.Combine(AppContext.BaseDirectory, "CleanArcClientFeature.db");

//        if (File.Exists(dbPath))
//        {
//            File.Delete(dbPath);
//            Console.WriteLine($"🗑️  Banco de dados removido: {dbPath}");
//        }

//        // Recriar conexão
//        using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
//        connection.Open();

//        using var cmd = new SQLiteCommand(
//            @"CREATE TABLE Clients (
//                Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                NomeFantasia TEXT NOT NULL,
//                Cnpj TEXT NOT NULL,
//                Ativo BOOLEAN NOT NULL
//            )", connection);

//        cmd.ExecuteNonQuery();

//        return Results.Ok(new
//        {
//            Success = true,
//            Message = "Banco de dados resetado com sucesso!",
//            DatabasePath = dbPath
//        });
//    }
//    catch (Exception ex)
//    {
//        return Results.Problem($"❌ Erro ao resetar banco: {ex.Message}");
//    }
//});

app.Run();