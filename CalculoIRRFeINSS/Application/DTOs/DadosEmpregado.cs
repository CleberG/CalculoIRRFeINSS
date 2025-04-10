using CalculoIRRFeINSS.Domain.Entidades;

namespace CalculoIRRFeINSS.Application.DTOs
{
    public class DadosEmpregado
    {
        public string NomeEmpregado { get; set; }
        public decimal Proventos { get; set; }
        public decimal Descontos { get; set; }
        public decimal BaseINSS { get; set; }
        public decimal Inss { get; set; }
        public decimal BaseIRRF { get; set; }
        public decimal Irrf { get; set; }
        public decimal Dependentes { get; set; }
        public decimal SalarioLiquido { get; set; }

        public static DadosEmpregado ObeterDadoEmpregado(Empregado empregado)
        {
            const decimal salarioMinimo = 1518.00m;

            var proventos = empregado.ObterTotalProventos();
            var descontos = empregado.ObterTotalDescontos();
            var baseINSS = proventos - descontos;
            var inss = empregado.CalcularINSS();
            var valorDependentes = empregado.Dependentes * (salarioMinimo * 0.05m);
            var baseIR = baseINSS - inss - valorDependentes;
            var irrf = empregado.CalcularIRRF();
            var liquido = proventos - descontos - inss - irrf;

            return new DadosEmpregado
            {
                NomeEmpregado = empregado.Nome,
                Proventos = proventos,
                Descontos = descontos,
                BaseINSS = baseINSS,
                Inss = inss,
                BaseIRRF = Math.Round(baseIR, 2),
                Irrf = irrf,
                Dependentes = Math.Round(valorDependentes, 2),
                SalarioLiquido = liquido
            };

        }
    }
}
