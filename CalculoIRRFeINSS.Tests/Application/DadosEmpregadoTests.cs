using CalculoIRRFeINSS.Application.DTOs;
using CalculoIRRFeINSS.Domain.Entidades;
using CalculoIRRFeINSS.Domain.Enuns;

namespace CalculoIRRFeINSS.Tests.Application
{
    public class DadosEmpregadoTests
    {
        [Fact]
        public void ObeterDadoEmpregado_DeveCalcularDadosCorretamente()
        {
            var empregado = new Empregado();
            empregado.SetEmpregado(1, "Ana", "12345678901", 2);
            empregado.AdicionarRubrica(new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 5000m));
            empregado.AdicionarRubrica(new Rubrica(2, 2001, "INSS", TipoRubrica.Desconto, 300m));

            var dto = DadosEmpregado.ObeterDadoEmpregado(empregado);

            Assert.Equal("Ana", dto.NomeEmpregado);
            Assert.Equal(5000m, dto.Proventos);
            Assert.Equal(300m, dto.Descontos);
            Assert.Equal(5000m - 300m, dto.BaseINSS);

            Assert.True(dto.Inss > 0);
            Assert.True(dto.BaseIRRF > 0);
            Assert.True(dto.Irrf >= 0);

            Assert.Equal(2 * (1518m * 0.05m), dto.Dependentes);
            Assert.Equal(dto.Proventos - dto.Descontos - dto.Inss - dto.Irrf, dto.SalarioLiquido);
        }

        [Fact]
        public void ObeterDadoEmpregado_DeveRetornarZeros_ParaEmpregadoSemRubricas()
        {
            var empregado = new Empregado();
            empregado.SetEmpregado(1, "José", "98765432100", 0);

            var dto = DadosEmpregado.ObeterDadoEmpregado(empregado);

            Assert.Equal("José", dto.NomeEmpregado);
            Assert.Equal(0m, dto.Proventos);
            Assert.Equal(0m, dto.Descontos);
            Assert.Equal(0m, dto.BaseINSS);
            Assert.Equal(0m, dto.Inss);
            Assert.Equal(0m, dto.BaseIRRF);
            Assert.Equal(0m, dto.Irrf);
            Assert.Equal(0m, dto.Dependentes);
            Assert.Equal(0m, dto.SalarioLiquido);
        }
    }
}
