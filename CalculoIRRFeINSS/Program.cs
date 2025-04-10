using System;
using System.Threading.Tasks;
using CalculoIRRFeINSS.Application;
using CalculoIRRFeINSS.Domain.Entidades;
using CalculoIRRFeINSS.Domain.Validators;
using CalculoIRRFeINSS.Infra;
using FluentValidation;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string caminhoArquivo = "C:\\Users\\Cleber\\Documents\\wk\\calculoFolha.txt";

            var leitor = new LeitorAquivoCSV();
            IValidator<Empregado> empregadoValidator = new EmpregadoValidator();
            IValidator<Rubrica> rubricaValidator = new RubricaValidator();

            var processador = new ProcessadorFolhaAppService(leitor, empregadoValidator, rubricaValidator);

            var dadosEmpregado = await processador.ExecutarAsync(caminhoArquivo);

            Console.WriteLine($"Empregado: {dadosEmpregado.NomeEmpregado}");
            Console.WriteLine($"Base de cálculo INSS: {dadosEmpregado.BaseINSS}");
            Console.WriteLine($"Base de cálculo IRRF: {dadosEmpregado.BaseIRRF}");
            Console.WriteLine($"Desconto dos dependentes: {dadosEmpregado.Dependentes}");
            Console.WriteLine($"Valor do INSS: {dadosEmpregado.Inss}");
            Console.WriteLine($"Valor IRRF: {dadosEmpregado.Irrf}");
            Console.WriteLine($"Valor líquido do salário: {dadosEmpregado.SalarioLiquido}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
        }

        Console.WriteLine("\nPressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}
