using CalculoIRRFeINSS.Domain.Entidades;

namespace CalculoIRRFeINSS.Domain.Interfaces
{
    public interface ILeitoArquivoCSV
    {
        Task<Empregado> LerEmpregado(string caminhoArquivo);
    }
}
