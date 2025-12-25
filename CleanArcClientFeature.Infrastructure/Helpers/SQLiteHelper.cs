using System.Data;
using System.Data.SQLite;

namespace CleanArcClientFeature.Infrastructure.Helpers;

public static class SQLiteHelper
{
    private static SQLiteConnection? _conexaoCompartilhada;
    private static readonly object _lock = new object();

    public static SQLiteConnection ObterConexãoCompartilhada()
    {
        lock (_lock)
        {
            if (_conexaoCompartilhada == null || _conexaoCompartilhada.State != ConnectionState.Open)
            {
                var dbCaminho = Path.Combine(AppContext.BaseDirectory, "CleanArcClientFeature.db");
                var conexaoString = $"Data Source={dbCaminho};Version=3;";

                _conexaoCompartilhada = new SQLiteConnection(conexaoString);
                _conexaoCompartilhada.Open();

                // Criar tabelas
                CreateTables(_conexaoCompartilhada);
            }
            return _conexaoCompartilhada;
        }
    }

    private static void CreateTables(SQLiteConnection conexao)
    {
        using var cmd = new SQLiteCommand(
            @"CREATE TABLE IF NOT EXISTS Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NomeFantasia TEXT NOT NULL,
                    Cnpj TEXT NOT NULL,
                    Ativo BOOLEAN NOT NULL
                )", conexao);
        cmd.ExecuteNonQuery();
    }
}