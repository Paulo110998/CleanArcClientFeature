namespace CleanArcClientFeature.Domain.ValueObjects;

public class Cnpj
{
    public string Value { get; protected set; }

    // Construtor protegido para o NHibernate - NÃO valida durante inicialização
    protected Cnpj()
    {
        Value = string.Empty;
    }

    // Construtor público com validação
    public Cnpj(string value)
    {
        if (value == null)
            throw new ArgumentException("CNPJ não pode ser nulo");

        Value = value;
    }

    // Método estático para criar com validação completa
    public static Cnpj Criar(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CNPJ inválido");

        return new Cnpj(value);
    }

    public override bool Equals(object? obj)
        => obj is Cnpj other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();
}