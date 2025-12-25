using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using CleanArcClientFeature.Domain.Entities;
using CleanArcClientFeature.Infrastructure.Types;


namespace CleanArcClientFeature.Infrastructure.Mappings;  

public class ClienteMap : ClassMapping<Cliente>
{
    public ClienteMap()
    {
        Table("Clients");
        Lazy(false);

        Id(x => x.Id, m =>
        {
            m.Generator(Generators.Identity);
        });

        Property(x => x.NomeFantasia, m =>
        {
            m.Length(100);
            m.NotNullable(true);
        });

        // **IMPORTANTE: Verifique se o CnpjType está acessível**
        Property(x => x.Cnpj, m =>
        {
            m.Column("Cnpj");
            m.Type<CnpjTipo>();
            m.Length(14);
            m.NotNullable(true);
        });

        Property(x => x.Ativo, m =>
        {
            m.NotNullable(true);
        });
    }
}