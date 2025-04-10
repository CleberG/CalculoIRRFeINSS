using CalculoIRRFeINSS.Domain.Entidades;
using CalculoIRRFeINSS.Domain.Enuns;
using CalculoIRRFeINSS.Domain.Interfaces;
using System.Globalization;

namespace CalculoIRRFeINSS.Infra
{
    internal class LeitorAquivoCSV : ILeitoArquivoCSV
    {
        public LeitorAquivoCSV()
        {
        }

        public async Task<Empregado> LerEmpregado(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException($"Arquivo não encontrado: {caminhoArquivo}");


            using var stream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            using var reader = new StreamReader(stream);

            string? linha;
            int numeroLinha = 0;
            var empregado = new Empregado();
            while ((linha = await reader.ReadLineAsync()) != null)
            {
                numeroLinha++;

                if (string.IsNullOrWhiteSpace(linha)) continue;

                var campos = linha.Split(';');

                if (campos.Length < 4)
                    throw new FormatException($"Linha {numeroLinha} inválida: número de colunas insuficiente.");

                var tipo = campos[0].Trim().ToUpper();

                if (tipo == "EMP")
                {
                    var id = 1;
                    var nome = campos[2].Trim();
                    var cpf = campos[1];
                    var dependentes = int.Parse(campos[3].Trim());

                    empregado?.SetEmpregado(id, nome, cpf, dependentes);
                }
                else if (tipo == "RUB")
                {
                    var rubricas = new List<Rubrica>();

                    int id = rubricas.Count + 1;
                    string descricao = campos[2].Trim();
                    decimal valor = ParseDecimal(campos[4], "Valor", numeroLinha);
                    string tipoRubricaStr = campos[3].Trim().ToUpper();
                    var codSeguranca = ParseInt(campos[1], "Cód segurança", numeroLinha);
                    var tipoRubrica = GetTipoRubrica(tipoRubricaStr);

                    empregado?.AdicionarRubrica(new Rubrica(id, codSeguranca, descricao, tipoRubrica, valor));
                }
                else
                {
                    throw new Exception($"Tipo de registro desconhecido na linha {numeroLinha}: {tipo}");
                }
            }

            return empregado;
        }

        private static TipoRubrica GetTipoRubrica(string tipoRubricaStr)
        {
            if (string.IsNullOrWhiteSpace(tipoRubricaStr))
                throw new ArgumentException("Tipo de rubrica está vazio ou nulo.");

            return tipoRubricaStr.Trim().ToUpperInvariant()[0] switch
            {
                'P' => TipoRubrica.Provento,
                'D' => TipoRubrica.Desconto,
                _ => throw new ArgumentException($"Tipo de rubrica inválido: {tipoRubricaStr}")
            };
        }

        private static int ParseInt(string valor, string campo, int linha)
        {
            if (!int.TryParse(valor, out int resultado))
                throw new FormatException($"Valor inválido para {campo} na linha {linha}: {valor}");

            return resultado;
        }

        private static long ParseLong(string valor, string campo, int linha)
        {
            if (!long.TryParse(valor, out long resultado))
                throw new FormatException($"Valor inválido para {campo} na linha {linha}: {valor}");

            return resultado;
        }

        private static decimal ParseDecimal(string valor, string campo, int linha)
        {
            if (!decimal.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture, out var resultado))
                throw new FormatException($"Valor inválido para {campo} na linha {linha}: {valor}");

            return resultado;
        }

        private static bool EhCpfValido(long cpf)
        {
            return cpf.ToString().Length == 11;
        }
    }
}
