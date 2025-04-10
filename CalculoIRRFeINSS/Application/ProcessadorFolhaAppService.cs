using CalculoIRRFeINSS.Application.DTOs;
using CalculoIRRFeINSS.Domain.Entidades;
using CalculoIRRFeINSS.Domain.Interfaces;
using FluentValidation;

namespace CalculoIRRFeINSS.Application
{
    public class ProcessadorFolhaAppService
    {
        private readonly ILeitoArquivoCSV _leitor;
        private readonly IValidator<Empregado> _empregadoValidator;
        private readonly IValidator<Rubrica> _rubricaValidator;

        public ProcessadorFolhaAppService(
            ILeitoArquivoCSV leitor,
            IValidator<Empregado> empregadoValidator,
            IValidator<Rubrica> rubricaValidator)
        {
            _leitor = leitor;
            _empregadoValidator = empregadoValidator;
            _rubricaValidator = rubricaValidator;
        }

        public async Task<DadosEmpregado> ExecutarAsync(string caminhoArquivo)
        {
            var empregado = await _leitor.LerEmpregado(caminhoArquivo);

            ValidarEmpregado(empregado);

            var dadosEmpregado = DadosEmpregado.ObeterDadoEmpregado(empregado);

            return dadosEmpregado;
        }

        private void ValidarEmpregado(Empregado empregado)
        {
            var resultadoEmpregado = _empregadoValidator.Validate(empregado);
            if (!resultadoEmpregado.IsValid)
            {
                var mensagens = string.Join("\n", resultadoEmpregado.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erros no empregado:\n{mensagens}");
            }

            foreach (var rubrica in empregado.Rubricas)
            {
                var resultadoRubrica = _rubricaValidator.Validate(rubrica);
                if (!resultadoRubrica.IsValid)
                {
                    var mensagens = string.Join("\n", resultadoRubrica.Errors.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Erros na rubrica '{rubrica.Descricao}':\n{mensagens}");
                }
            }
        }
    }
}
