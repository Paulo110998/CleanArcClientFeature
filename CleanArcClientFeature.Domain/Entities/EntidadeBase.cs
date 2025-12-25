namespace CleanArcClientFeature.Domain.Entities;

public abstract class EntidadeBase
{
    public virtual int Id { get; protected set; }

    // Método protegido para o NHibernate setar o Id
    protected virtual void SetId(int id) { Id = id; }
}