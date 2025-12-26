using CleanArcClientFeature.Infrastructure.Helpers;
using CleanArcClientFeature.Infrastructure.Interfaces;
using System.Data.SQLite;

namespace CleanArcClientFeature.Infrastructure.Config;

public class DatabaseConfig : IDatabaseConfig
{
    private readonly string _databaseName;
    private SQLiteConnection? _conexaoCompartilhada;

    public DatabaseConfig(string databaseName = "CleanArcClientFeature.db")
    {
        _databaseName = databaseName;
    }

    public SQLiteConnection CriarConexaoCompartilhada()
    {
        if (_conexaoCompartilhada == null)
        {
            _conexaoCompartilhada = DatabaseHelper.CriarConexaoCompartilhada(_databaseName);
        }
        return _conexaoCompartilhada;
    }

    public void CriarTabelasSeNaoExistirem()
    {
        var conexao = CriarConexaoCompartilhada();
        DatabaseHelper.CriarTabelasSeNaoExistirem(conexao);
    }
}