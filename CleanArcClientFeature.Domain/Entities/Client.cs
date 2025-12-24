//// Domain/Entities/Client.cs
//using CleanArcClientFeature.Domain.Validation;
//using CleanArcClientFeature.Domain.ValueObjects;

//namespace CleanArcClientFeature.Domain.Entities;

//public class Client : EntityBase
//{
//    public virtual string NomeFantasia { get; protected set; }
//    public virtual Cnpj Cnpj { get; protected set; }
//    public virtual bool Ativo { get; protected set; }

//    // Construtor sem parâmetros para o NHibernate
//    protected Client()
//    {
//        // Usado apenas pelo ORM
//    }

//    // Contrutor 1
//    public Client(string nomeFantasia, Cnpj cnpj, bool ativo)
//    {
//        ValidateDomain(nomeFantasia, cnpj, ativo);
//    }

//    // Construtor 2
//    public Client(int id, string nomeFantasia, Cnpj cnpj, bool ativo)
//    {
//        DomainExceptionValidation.When(Id < 0, "Invalid Id Value");
//        Id = id;
//        ValidateDomain(nomeFantasia, cnpj, ativo);
//    }

//    // Método Update também precisa ser virtual
//    public virtual void Update(string nomeFantasia, Cnpj cnpj, bool ativo)
//    {
//        ValidateDomain(nomeFantasia, cnpj, ativo);
//    }

//    private void ValidateDomain(string nomeFantasia, Cnpj cnpj, bool ativo)
//    {
//        DomainExceptionValidation.When(
//            string.IsNullOrWhiteSpace(nomeFantasia),
//            "Invalid Nome Fantasia. Is required"
//        );

//        DomainExceptionValidation.When(
//            cnpj is null,
//            "CNPJ is required"
//        );

//        NomeFantasia = nomeFantasia;
//        Cnpj = cnpj;
//        Ativo = ativo;
//    }
//}


using CleanArcClientFeature.Domain.Validation;
using CleanArcClientFeature.Domain.ValueObjects;

namespace CleanArcClientFeature.Domain.Entities
{
    public class Client : EntityBase
    {
        
        public string NomeFantasia { get; protected set; }
        public Cnpj Cnpj { get; protected set; }
        public bool Ativo { get; protected set; }

        // Construtor para NHibernate
        protected Client()
        {
            NomeFantasia = string.Empty;
            Cnpj = new Cnpj(string.Empty);
            Ativo = false;
        }

        public Client(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            ValidateDomain(nomeFantasia, cnpj, ativo);
        }

        public Client(int id, string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            DomainExceptionValidation.When(Id < 0, "Invalid Id Value");
            Id = id;
            ValidateDomain(nomeFantasia, cnpj, ativo);
        }

        public void Update(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            ValidateDomain(nomeFantasia, cnpj, ativo);
        }

        private void ValidateDomain(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            DomainExceptionValidation.When(
                string.IsNullOrWhiteSpace(nomeFantasia),
                "Invalid Nome Fantasia. Is required"
            );

            DomainExceptionValidation.When(
                cnpj is null,
                "CNPJ is required"
            );

            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            Ativo = ativo;
        }
    }
}