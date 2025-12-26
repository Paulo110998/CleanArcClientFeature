using System.Data.SQLite;


namespace CleanArcClientFeature.Infrastructure.Interfaces;

public interface IDatabaseConfig
{
    SQLiteConnection CriarConexaoCompartilhada();
    void CriarTabelasSeNaoExistirem();
}
