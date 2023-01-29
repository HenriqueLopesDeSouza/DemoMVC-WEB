using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class LivroRepository : Repository<Livro>, ILivroRepository
    {
        public LivroRepository(MeuDbContext context) : base(context) { }

        public async Task<Livro> ObterLivroAutor(Guid id)
        {
            return await Db.Livros.AsNoTracking().Include(f => f.Autor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Livro>> ObterLivroAuthor()
        {
            return await Db.Livros.AsNoTracking().Include(f => f.Autor)
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Livro>> ObterLivrosPorAutor(Guid autorId)
        {
            return await Buscar(p => p.AutorId == autorId);
        }
    }
}