using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using CleanArcClientFeature.Domain.Entities;


namespace CleanArcClientFeature.Infrastructure.Mappings;  

public class ClientMap : ClassMapping<Client>
{
    public ClientMap()
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
            m.Type<CnpjType>();
            m.Length(14);
            m.NotNullable(true);
        });

        Property(x => x.Ativo, m =>
        {
            m.NotNullable(true);
        });
    }
}