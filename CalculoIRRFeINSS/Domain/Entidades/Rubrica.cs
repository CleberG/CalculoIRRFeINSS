using CalculoIRRFeINSS.Domain.Enuns;

namespace CalculoIRRFeINSS.Domain.Entidades
{
    public class Rubrica
    {
        public int Id { get; private set; }
        public int Codigo { get; private set; }
        public string Descricao { get; private set; }
        public TipoRubrica TipoRubrica { get; private set; }
        public decimal Valor { get; private set; }

        public Rubrica(int id, int codigo, string descricao, TipoRubrica tipoRubrica, decimal valor)
        {
            Id = id;
            Codigo = codigo;
            Descricao = descricao;
            TipoRubrica = tipoRubrica;
            Valor = valor;
        }
    }
}
