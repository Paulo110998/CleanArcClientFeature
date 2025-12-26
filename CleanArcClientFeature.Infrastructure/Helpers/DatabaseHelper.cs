
using System.Data.SQLite;

namespace CleanArcClientFeature.Infrastructure.Helpers;

public static class DatabaseHelper
{
    public static SQLiteConnection CriarConexaoCompartilhada(string databaseName = "CleanArcClientFeature.db")
    {
        // Usa arquivo físico para desenvolvimento (mais estável)
        var dbCaminho = Path.Combine(AppContext.BaseDirectory, databaseName);
        var conexaoString = $"Data Source={dbCaminho};Version=3;";

        Console.WriteLine($"Banco de dados: {dbCaminho}");

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

    public static void CriarTabelasSeNaoExistirem(SQLiteConnection conexao)
    {
        try
        {
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

            // Verificar se a tabela existe
            using (var cmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Clients'",
                 conexao))
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

    public static void InicializarBancoDados(string databaseName = "CleanArcClientFeature.db")
    {
        var conexao = CriarConexaoCompartilhada(databaseName);
        CriarTabelasSeNaoExistirem(conexao);

        // Não feche a conexão aqui, ela será gerenciada pela aplicação
        Console.WriteLine("Banco de dados inicializado com sucesso!");
    }
}