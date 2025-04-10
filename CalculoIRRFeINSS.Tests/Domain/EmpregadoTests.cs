using CalculoIRRFeINSS.Domain.Entidades;
using CalculoIRRFeINSS.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRFeINSS.Tests.Domain
{
    public class EmpregadoTests
    {
        [Fact]
        public void SetEmpregado_DeveAtribuirPropriedadesCorretamente()
        {
            var empregado = new Empregado();
            empregado.SetEmpregado(1, "João", "12345678901", 2);

            Assert.Equal(1, empregado.Id);
            Assert.Equal("João", empregado.Nome);
            Assert.Equal("12345678901", empregado.CPF);
            Assert.Equal(2, empregado.Dependentes);
        }

        [Fact]
        public void AdicionarRubrica_DeveAdicionarRubricaNaLista()
        {
            var empregado = new Empregado();
            var rubrica = new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 3000m);

            empregado.AdicionarRubrica(rubrica);

            Assert.Single(empregado.Rubricas);
            Assert.Equal("Salário", empregado.Rubricas[0].Descricao);
        }

        [Fact]
        public void ObterTotalProventos_DeveSomarSomenteProventos()
        {
            var empregado = new Empregado();
            empregado.AdicionarRubrica(new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 3000m));
            empregado.AdicionarRubrica(new Rubrica(2, 1002, "Bônus", TipoRubrica.Provento, 500m));
            empregado.AdicionarRubrica(new Rubrica(3, 2001, "INSS", TipoRubrica.Desconto, 300m));

            var total = empregado.ObterTotalProventos();

            Assert.Equal(3500m, total);
        }

        [Fact]
        public void ObterTotalDescontos_DeveSomarSomenteDescontos()
        {
            var empregado = new Empregado();
            empregado.AdicionarRubrica(new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 3000m));
            empregado.AdicionarRubrica(new Rubrica(2, 2001, "INSS", TipoRubrica.Desconto, 300m));

            var total = empregado.ObterTotalDescontos();

            Assert.Equal(300m, total);
        }

        [Fact]
        public void CalcularINSS_DeveRetornarZero_SeBaseNegativa()
        {
            var empregado = new Empregado();
            empregado.AdicionarRubrica(new Rubrica(1, 2001, "Desconto extra", TipoRubrica.Desconto, 1000m));

            var inss = empregado.CalcularINSS();

            Assert.Equal(0, inss);
        }

        [Fact]
        public void CalcularINSS_DeveCalcularCorretamente()
        {
            var empregado = new Empregado();
            empregado.AdicionarRubrica(new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 3000m));

            var inss = empregado.CalcularINSS();

            Assert.Equal(360.00m, inss);
        }

        [Fact]
        public void BaseCalculoIRRF_DeveCalcularCorretamente()
        {
            var empregado = new Empregado();
            empregado.SetEmpregado(1, "João", "12345678901", 2);
            empregado.AdicionarRubrica(new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 5000m));
            empregado.AdicionarRubrica(new Rubrica(2, 2001, "INSS", TipoRubrica.Desconto, 0m));

            var baseIR = empregado.BaseCalculoIRRF();

            Assert.True(baseIR == 4148.20m);
        }

        [Fact]
        public void CalcularIRRF_DeveRetornarZero_SeBaseForNegativa()
        {
            var empregado = new Empregado();
            empregado.SetEmpregado(1, "João", "12345678901", 1);
            empregado.AdicionarRubrica(new Rubrica(1, 2001, "Desconto", TipoRubrica.Desconto, 2000m));

            var irrf = empregado.CalcularIRRF();

            Assert.Equal(0, irrf);
        }
    }
}
