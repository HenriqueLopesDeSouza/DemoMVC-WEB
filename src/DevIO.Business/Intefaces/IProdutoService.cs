using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface ILivroService : IDisposable
    {
        Task Adicionar(Livro produto);
        Task Atualizar(Livro produto);
        Task Remover(Guid id);
    }
}