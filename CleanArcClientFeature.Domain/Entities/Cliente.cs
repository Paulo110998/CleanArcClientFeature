using CleanArcClientFeature.Domain.Validation;
using CleanArcClientFeature.Domain.ValueObjects;

namespace CleanArcClientFeature.Domain.Entities
{
    public class Cliente : EntidadeBase
    {
        
        public virtual string NomeFantasia { get; protected set; }
        public virtual Cnpj Cnpj { get; protected set; }
        public virtual bool Ativo { get; protected set; }

        // Construtor para NHibernate
        protected Cliente()
        {
            NomeFantasia = string.Empty;
            Cnpj = new Cnpj(string.Empty);
            Ativo = false;
        }

        public Cliente(string nomeFantasia, Cnpj cnpj, bool ativo)
            : this() // Chama o construtor protegido primeiro
        {
            ValidarDomínio(nomeFantasia, cnpj, ativo);
        }

        public Cliente(int id, string nomeFantasia, Cnpj cnpj, bool ativo)
            : this() // Chama o construtor protegido primeiro
        {
            ExcecaoDeValidacaoDeDominio.ExcecaoDeErro(id < 0, "Invalid Id Value");
            Id = id;
            ValidarDomínio(nomeFantasia, cnpj, ativo);
        }

        public virtual void Atualizar(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            ValidarDomínio(nomeFantasia, cnpj, ativo);
        }

        private void ValidarDomínio(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            ExcecaoDeValidacaoDeDominio.ExcecaoDeErro(
                string.IsNullOrWhiteSpace(nomeFantasia),
                "Invalid Nome Fantasia. Is required"
            );

            ExcecaoDeValidacaoDeDominio.ExcecaoDeErro(
                cnpj is null,
                "CNPJ is required"
            );

            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            Ativo = ativo;
        }
    }
}