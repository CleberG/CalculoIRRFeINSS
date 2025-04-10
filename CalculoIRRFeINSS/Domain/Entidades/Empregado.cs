using CalculoIRRFeINSS.Domain.Enuns;
using CalculoIRRFeINSS.Domain.Helpers;

namespace CalculoIRRFeINSS.Domain.Entidades
{
    public class Empregado
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public int Dependentes { get; private set; }

        public IList<Rubrica> Rubricas { get; private set; } = [];

        public Empregado() { }

        public void SetEmpregado(int id, string nome, string cpf, int dependentes)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            Dependentes = dependentes;
        }

        public void AdicionarRubrica(Rubrica rubrica)
        {
            Rubricas.Add(rubrica);
        }

        public decimal ObterTotalProventos()
        {
            return Rubricas
                .Where(r => r.TipoRubrica == TipoRubrica.Provento)
                .Sum(r => r.Valor);
        }

        public decimal ObterTotalDescontos()
        {
            return Rubricas
                .Where(r => r.TipoRubrica == TipoRubrica.Desconto)
                .Sum(r => r.Valor);
        }

        public decimal CalcularINSS()
        {
            var baseCalculo = ObterTotalProventos() - ObterTotalDescontos();

            if (baseCalculo <= 0)
                return 0;

            var percentual = TabelaINSS.ObterPercentual(baseCalculo);
            var inss = baseCalculo * percentual;

            return Math.Round(inss, 2);
        }

        public decimal CalcularIRRF()
        {
            var baseIRRF = BaseCalculoIRRF();

            if (baseIRRF <= 0)
                return 0;

            var percentual = TabelaIRRF.ObterPercentual(baseIRRF);
            var irrf = baseIRRF * percentual;

            return Math.Round(irrf, 2);
        }

        public decimal BaseCalculoIRRF()
        {
            const decimal salarioMinimo = 1518.00m;

            var totalProventos = ObterTotalProventos();
            var totalDescontos = ObterTotalDescontos();
            var inss = CalcularINSS();
            var valorDependentes = Dependentes * (salarioMinimo * 0.05m);

            return totalProventos - totalDescontos - inss - valorDependentes;
        }
    }
}
