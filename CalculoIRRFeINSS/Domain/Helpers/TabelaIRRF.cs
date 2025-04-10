namespace CalculoIRRFeINSS.Domain.Helpers
{
    public class TabelaIRRF
    {
        private const decimal SalarioMinimo = 1518.00m;

        private static readonly List<(decimal Limite, decimal Percentual)> Faixas = new()
        {
            (SalarioMinimo * 2, 0.00m),
            (SalarioMinimo * 4, 0.05m),
            (SalarioMinimo * 5, 0.10m),
            (SalarioMinimo * 7, 0.15m),
            (decimal.MaxValue, 0.20m)
        };

        public static decimal ObterPercentual(decimal baseIR)
        {
            if (baseIR <= 0) return 0;
            return Faixas.First(f => baseIR <= f.Limite).Percentual;
        }
    }
}
