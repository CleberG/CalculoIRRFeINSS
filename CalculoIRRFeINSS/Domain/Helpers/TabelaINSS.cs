namespace CalculoIRRFeINSS.Domain.Helpers
{
    public class TabelaINSS
    {
        private const decimal SalarioMinimo = 1518.00m;

        private static readonly List<(decimal Limite, decimal Percentual)> Faixas = new()
        {
            (SalarioMinimo, 0.075m),                        
            (2793.88m, 0.09m),                              
            (4190.83m, 0.12m),                              
            (decimal.MaxValue, 0.14m)                       
        };

        public static decimal ObterPercentual(decimal baseINSS)
        {
            if (baseINSS <= 0) return 0;
            return Faixas.First(f => baseINSS <= f.Limite).Percentual;
        }
    }
}
