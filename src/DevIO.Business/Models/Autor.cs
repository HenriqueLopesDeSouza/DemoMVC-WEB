using System;
using System.Collections.Generic;

namespace DevIO.Business.Models
{
    public class Autor : Entity
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }

        /* EF Relations */
        public IEnumerable<Livro> Livros { get; set; }
    }
}