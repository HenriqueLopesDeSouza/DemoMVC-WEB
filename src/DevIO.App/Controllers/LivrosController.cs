using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class LivrosController : BaseController
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly ILivroService _livroService;
        private readonly IMapper _mapper;

        public LivrosController(ILivroRepository livroRepository, 
                                  IAutorRepository autorRepository, 
                                  IMapper mapper,
                                  ILivroService livroService,
                                  INotificador notificador) : base(notificador)
        {
            _livroRepository = livroRepository;
            _autorRepository = autorRepository;
            _mapper = mapper;
            _livroService = livroService;
        }

        [AllowAnonymous]
        [Route("lista-de-livros")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<LivroViewModel>>(await _livroRepository.ObterLivroAuthor()));
        }

        [AllowAnonymous]
        [Route("dados-do-livros/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var livroViewModel = await ObterLivro(id);

            if (livroViewModel == null)
            {
                return NotFound();
            }

            return View(livroViewModel);
        }

        [AllowAnonymous]
        [Route("novo-livro")]
        public async Task<IActionResult> Create()
        {
            var livroViewModel = await PopularAutores(new LivroViewModel());

            return View(livroViewModel);
        }

        [AllowAnonymous]
        [Route("novo-livro")]
        [HttpPost]
        public async Task<IActionResult> Create(LivroViewModel livroViewModel)
        {
            livroViewModel = await PopularAutores(livroViewModel);
            if (!ModelState.IsValid) return View(livroViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(livroViewModel.ImagemUpload, imgPrefixo))
            {
                return View(livroViewModel);
            }

            livroViewModel.Imagem = imgPrefixo + livroViewModel.ImagemUpload.FileName;
            await _livroService.Adicionar(_mapper.Map<Livro>(livroViewModel));

            if (!OperacaoValida()) return View(livroViewModel);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Route("editar-livro/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var livroViewModel = await ObterLivro(id);

            if (livroViewModel == null)
            {
                return NotFound();
            }

            return View(livroViewModel);
        }

        [AllowAnonymous]
        [Route("editar-livro/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, LivroViewModel livroViewModel)
        {
            if (id != livroViewModel.Id) return NotFound();

            var livroAtualizacao = await ObterLivro(id);
            livroViewModel.Autor = livroAtualizacao.Autor;
            livroViewModel.Imagem = livroAtualizacao.Imagem;
            if (!ModelState.IsValid) return View(livroViewModel);

            if (livroViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(livroViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(livroViewModel);
                }

                livroAtualizacao.Imagem = imgPrefixo + livroViewModel.ImagemUpload.FileName;
            }

            livroAtualizacao.Nome = livroViewModel.Nome;
            livroAtualizacao.Descricao = livroViewModel.Descricao;
            livroAtualizacao.Valor = livroViewModel.Valor;

            await _livroService.Atualizar(_mapper.Map<Livro>(livroAtualizacao));

            if (!OperacaoValida()) return View(livroViewModel);

            return RedirectToAction("Index");
        }


        [Route("excluir-livro/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var livro = await ObterLivro(id);

            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        [Route("excluir-livro/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var livro = await ObterLivro(id);

            if (livro == null)
            {
                return NotFound();
            }

            await _livroService.Remover(id);

            if (!OperacaoValida()) return View(livro);

            TempData["Sucesso"] = "Livro excluido com sucesso!";

            return RedirectToAction("Index");
        }

        private async Task<LivroViewModel> ObterLivro(Guid id)
        {
            var livro = _mapper.Map<LivroViewModel>(await _livroRepository.ObterLivroAutor(id));
            livro.Autores = _mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos());
            return livro;
        }

        private async Task<LivroViewModel> PopularAutores(LivroViewModel livro)
        {
            livro.Autores = _mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos());
            return livro;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
