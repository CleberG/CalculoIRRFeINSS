using CalculoIRRFeINSS.Application;
using CalculoIRRFeINSS.Domain.Entidades;
using CalculoIRRFeINSS.Domain.Enuns;
using CalculoIRRFeINSS.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRFeINSS.Tests.Application
{
    public class ProcessadorFolhaTests
    {
        [Fact]
        public async Task ExecutarAsync_DeveRetornarDadosEmpregado_SeValido()
        {
            // Arrange
            var empregado = new Empregado();
            empregado.SetEmpregado(1, "João", "12345678901", 1);
            empregado.AdicionarRubrica(new Rubrica(1, 1001, "Salário", TipoRubrica.Provento, 4000m));
            empregado.AdicionarRubrica(new Rubrica(2, 2001, "INSS", TipoRubrica.Desconto, 300m));

            var leitor = Substitute.For<ILeitoArquivoCSV>();
            leitor.LerEmpregado(Arg.Any<string>()).Returns(empregado);

            var validadorEmpregado = Substitute.For<IValidator<Empregado>>();
            validadorEmpregado.Validate(empregado).Returns(new ValidationResult());

            var validadorRubrica = Substitute.For<IValidator<Rubrica>>();
            validadorRubrica.Validate(Arg.Any<Rubrica>()).Returns(new ValidationResult());

            var service = new ProcessadorFolhaAppService(leitor, validadorEmpregado, validadorRubrica);

            // Act
            var resultado = await service.ExecutarAsync("qualquer_arquivo.csv");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(4000m, resultado.Proventos);
            Assert.Equal(300m, resultado.Descontos);
        }
    }
}
