using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Task<Autor> ObterAutorEndereco(Guid id);
        Task<Autor> ObterAutorLivrosEndereco(Guid id);
    }
}