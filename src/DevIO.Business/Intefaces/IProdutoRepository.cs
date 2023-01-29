using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface ILivroRepository : IRepository<Livro>
    {
        Task<IEnumerable<Livro>> ObterLivrosPorAutor(Guid autorId);
        Task<IEnumerable<Livro>> ObterLivroAuthor();
        Task<Livro> ObterLivroAutor(Guid id);
    }
}