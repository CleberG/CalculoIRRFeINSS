using CalculoIRRFeINSS.Domain.Enuns;
using CalculoIRRFeINSS.Infra;

namespace CalculoIRRFeINSS.Tests.Infra
{
    public class LeitorArquivoCSVTests
    {
        private async Task<string> CriarArquivoTemporarioAsync(string[] linhas)
        {
            var caminho = Path.GetTempFileName();
            await File.WriteAllLinesAsync(caminho, linhas);
            return caminho;
        }

        [Fact]
        public async Task LerEmpregado_DeveLerArquivoValidoComRubricas()
        {
            var linhas = new[]
            {
                "EMP;12345678901;Ana;2",
                "RUB;1001;Salario;P;5000.00",
                "RUB;2001;INSS;D;300.00"
            };

            var caminho = await CriarArquivoTemporarioAsync(linhas);
            var leitor = new LeitorAquivoCSV();

            var empregado = await leitor.LerEmpregado(caminho);

            Assert.NotNull(empregado);
            Assert.Equal("Ana", empregado.Nome);
            Assert.Equal("12345678901", empregado.CPF);
            Assert.Equal(2, empregado.Dependentes);
            Assert.Equal(2, empregado.Rubricas.Count);
            Assert.Equal(TipoRubrica.Provento, empregado.Rubricas[0].TipoRubrica);
            Assert.Equal(TipoRubrica.Desconto, empregado.Rubricas[1].TipoRubrica);
        }

        [Fact]
        public async Task LerEmpregado_DeveLancarExcecao_SeArquivoNaoExiste()
        {
            var leitor = new LeitorAquivoCSV();

            await Assert.ThrowsAsync<FileNotFoundException>(() =>
                leitor.LerEmpregado("caminho/que/nao/existe.csv"));
        }

        [Fact]
        public async Task LerEmpregado_DeveLancarExcecao_SeLinhaComPoucosCampos()
        {
            var linhas = new[]
            {
                "EMP;12345678901"
            };

            var caminho = await CriarArquivoTemporarioAsync(linhas);
            var leitor = new LeitorAquivoCSV();

            var ex = await Assert.ThrowsAsync<FormatException>(() => leitor.LerEmpregado(caminho));
            Assert.Contains("número de colunas insuficiente", ex.Message);
        }

        [Fact]
        public async Task LerEmpregado_DeveLancarExcecao_SeTipoRegistroDesconhecido()
        {
            var linhas = new[]
            {
                "XYZ;12345678901;Ana;2"
            };

            var caminho = await CriarArquivoTemporarioAsync(linhas);
            var leitor = new LeitorAquivoCSV();

            var ex = await Assert.ThrowsAsync<Exception>(() => leitor.LerEmpregado(caminho));
            Assert.Contains("Tipo de registro desconhecido", ex.Message);
        }

        [Fact]
        public async Task LerEmpregado_DeveLancarExcecao_SeTipoRubricaInvalido()
        {
            var linhas = new[]
            {
                "EMP;12345678901;Ana;1",
                "RUB;1001;Bonus;X;1000.00"
            };

            var caminho = await CriarArquivoTemporarioAsync(linhas);
            var leitor = new LeitorAquivoCSV();

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => leitor.LerEmpregado(caminho));
            Assert.Contains("Tipo de rubrica inválido", ex.Message);
        }

        [Fact]
        public async Task LerEmpregado_DeveLancarExcecao_SeValorInvalido()
        {
            var linhas = new[]
            {
                "EMP;12345678901;Ana;1",
                "RUB;1001;Bonus;P;milreais"
            };

            var caminho = await CriarArquivoTemporarioAsync(linhas);
            var leitor = new LeitorAquivoCSV();

            var ex = await Assert.ThrowsAsync<FormatException>(() => leitor.LerEmpregado(caminho));
            Assert.Contains("Valor inválido para Valor", ex.Message);
        }
    }
}
