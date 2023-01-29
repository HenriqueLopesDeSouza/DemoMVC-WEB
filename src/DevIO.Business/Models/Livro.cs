using System;

namespace DevIO.Business.Models
{
    public class Livro : Entity
    {
        public Guid AutorId { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }

        /* EF Relations */
        public Autor Autor { get; set; }
    }
}