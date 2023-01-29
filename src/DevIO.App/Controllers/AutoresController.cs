using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevIO.App.Controllers
{
    public class AutoresController : BaseController
    {
        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;
        private readonly IMapper _mapper;

        public AutoresController(IAutorRepository autorRepository, 
                                      IMapper mapper,
                                      IAutorService autorService,
                                      INotificador notificador) : base(notificador)
        {
            _autorRepository = autorRepository;
            _mapper = mapper;
            _autorService = autorService;
        }


        [Route("lista-de-autor")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos()));
        }


        [Route("dados-do-autor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var autorViewModel = await ObterAutorEndereco(id);

            if (autorViewModel == null)
            {
                return NotFound();
            }

            return View(autorViewModel);
        }


        [Route("novo-autor")]
        public IActionResult Create()
        {
            return View();
        }


        [Route("novo-autor")]
        [HttpPost]
        public async Task<IActionResult> Create(AutorViewModel autorViewModel)
        {
            if (!ModelState.IsValid) return View(autorViewModel);

            var autor = _mapper.Map<Autor>(autorViewModel);
            await _autorService.Adicionar(autor);

            if (!OperacaoValida()) return View(autorViewModel);

            return RedirectToAction("Index");
        }


        [Route("editar-autor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var autorViewModel = await ObterAutorLivrosEndereco(id);

            if (autorViewModel == null)
            {
                return NotFound();
            }

            return View(autorViewModel);
        }


        [Route("editar-autor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AutorViewModel autorViewModel)
        {
            if (id != autorViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(autorViewModel);

            var autor = _mapper.Map<Autor>(autorViewModel);
            await _autorService.Atualizar(autor);

            if (!OperacaoValida()) return View(await ObterAutorLivrosEndereco(id));

            return RedirectToAction("Index");
        }


        [Route("excluir-autor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var autorViewModel = await ObterAutorEndereco(id);

            if (autorViewModel == null)
            {
                return NotFound();
            }

            return View(autorViewModel);
        }


        [Route("excluir-autor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var autor = await ObterAutorEndereco(id);

            if (autor == null) return NotFound();

            await _autorService.Remover(id);

            if (!OperacaoValida()) return View(autor);

            return RedirectToAction("Index");
        }


        [Route("obter-endereco-autor/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var autor = await ObterAutorEndereco(id);

            if (autor == null)
            {
                return NotFound();
            }

            return PartialView("_DetalhesEndereco", autor);
        }


        [Route("atualizar-endereco-autor/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var autor = await ObterAutorEndereco(id);

            if (autor == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new AutorViewModel { Endereco = autor.Endereco });
        }


        [Route("atualizar-endereco-autor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(AutorViewModel autorViewModel)
        {
            try
            {
                ModelState.Remove("Nome");
                ModelState.Remove("Documento");

                if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", autorViewModel);

                await _autorService.AtualizarEndereco(_mapper.Map<Endereco>(autorViewModel.Endereco));

                if (!OperacaoValida()) return PartialView("_AtualizarEndereco", autorViewModel);

                var url = Url.Action("ObterEndereco", "Autores", new { id = autorViewModel.Endereco.AutorId });
                return Json(new { success = true, url });
            }
            catch (Exception e) 
            {
                return Json(new { success = false, e.Message });

            }

        }

        private async Task<AutorViewModel> ObterAutorEndereco(Guid id)
        {
            return _mapper.Map<AutorViewModel>(await _autorRepository.ObterAutorEndereco(id));
        }

        private async Task<AutorViewModel> ObterAutorLivrosEndereco(Guid id)
        {
            return _mapper.Map<AutorViewModel>(await _autorRepository. ObterAutorLivrosEndereco(id));
        }
    }
}
