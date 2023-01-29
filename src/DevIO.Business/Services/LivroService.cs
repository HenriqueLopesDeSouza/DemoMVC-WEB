using System;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class LivroService : BaseService, ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository LivroRepository,
                              INotificador notificador) : base(notificador)
        {
            _livroRepository = LivroRepository;
        }

        public async Task Adicionar(Livro Livro)
        {
            if (!ExecutarValidacao(new LivroValidation(), Livro)) return;

            await _livroRepository.Adicionar(Livro);
        }

        public async Task Atualizar(Livro Livro)
        {
            if (!ExecutarValidacao(new LivroValidation(), Livro)) return;

            await _livroRepository.Atualizar(Livro);
        }

        public async Task Remover(Guid id)
        {
            await _livroRepository.Remover(id);
        }

        public void Dispose()
        {
            _livroRepository?.Dispose();
        }
    }
}