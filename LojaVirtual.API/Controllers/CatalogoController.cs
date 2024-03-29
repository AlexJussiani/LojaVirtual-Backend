﻿using AutoMapper;
using Loja.Core.Controllers;
using Loja.Core.Models;
using LojaVirtual.API.Data.Repository;
using LojaVirtual.API.Models;
using LojaVirtual.API.Services;
using LojaVirtual.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LojaVirtual.API.Controllers
{
   // [Authorize]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public CatalogoController(IProdutoRepository produtoRepository,
                                   IProdutoService produtoService,
                                   IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _produtoService = produtoService;
        }

        //[HttpGet("catalogo/produtos")]
        //public async Task<IActionResult> ObterProdutos()
        //{
        //    if (!ModelState.IsValid) return CustomResponse(ModelState);
        //    return CustomResponse(_mapper.Map<IEnumerable<ProdutoViewModel>> (await _produtoRepository.ObterTodosProdutos()));
        //}

        //[HttpGet("catalogo/paginado")]
        //public async Task<PagedResult<ProdutoViewModel>> ObterProdutos([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        //{
        //    //if (!ModelState.IsValid) return CustomResponse(ModelState);
        //    return _mapper.Map<PagedResult<ProdutoViewModel>>(await ObterPorPagina(ps, page, q));
            
        //}

        [HttpPost("catalogo/filtroPaginado")]
        public async Task<IActionResult> ObterProdutos([FromBody] List<FiltroViewModel> filtros, int ps = 8, int page = 1, OrdenacaoViewModel ordenacao = 0, string q = null)
        {
            //if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(_mapper.Map<PagedResult<ProdutoViewModel>>(await _produtoService.ObterPorPagina(filtros, ps, page, ordenacao, q)));            
        }

        [HttpGet("catalogo/produtosPorId/{id}")]
        public async Task<IActionResult> ObterProdutosPorId(Guid id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _produtoRepository.ObterProdutoPorId(id));
        }

        [HttpPost("catalogo/produtos")]
        public async Task<ActionResult<ProdutoViewModel>> AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
            if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustomResponse(produtoViewModel);
            }

            produtoViewModel.Imagem = imagemNome;
            await _produtoService.AdicionarProduto(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

            // [AllowAnonymous]
        [HttpGet("catalogo/marcas")]
        public async Task<IActionResult> ObterMarcas()
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _produtoRepository.ObterTodasMarcas());
        }

        [HttpPost("catalogo/marca")]
        public async Task<ActionResult<Marca>> AdicionarMarca(Marca marca)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.AdicionarMarca(marca);

            return CustomResponse(marca);
        }

        [HttpGet("catalogo/cores")]
        public async Task<IActionResult> ObterCores()
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _produtoRepository.ObterTodasCores());
        }

        [HttpGet("catalogo/tipoProduto")]
        public async Task<IActionResult> ObterTipoProduto()
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _produtoRepository.ObterTodosTipoProduto());
        }

        [HttpPost("catalogo/tipoProduto")]
        public async Task<ActionResult<Marca>> AdicionarTipoProduto(TipoProduto tipoProduto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.AdicionarTipoProduto(tipoProduto);

            return CustomResponse(tipoProduto);
        }

        [HttpGet("catalogo/tamanho")]
        public async Task<IActionResult> ObterTamanhos()
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            return CustomResponse(await _produtoRepository.ObterTodosTamanhos());
        }

        [HttpPost("catalogo/tamanho")]
        public async Task<ActionResult<Marca>> AdicionarTamanho(Tamanho tamanho)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.AdicionarTamanho(tamanho);

            return CustomResponse(tamanho);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                AdicionarErroProcessamento("Forneça uma imagem para este produto!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                AdicionarErroProcessamento("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private async Task<PagedResult<Produto>> ObterPorPagina(int pageSize, int pageIndex, string query = null)
        {
            var produtos = await _produtoRepository.ObterTodosProdutos(pageSize, pageIndex, query);
            return new PagedResult<Produto>()
            {
                List = produtos,
                TotalResults = _produtoRepository.ObterTodosProdutos().Result.Count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
        }

        //private async Task<PagedResult<Produto>> ObterPorPagina(List<FiltroViewModel> filtros,int pageSize, int pageIndex, string query = null)
        //{
        //    var produtos = await _produtoRepository.ObterTodosProdutos(pageSize, pageIndex, query);
        //    return new PagedResult<Produto>()
        //    {
        //        List = produtos,
        //        TotalResults = _produtoRepository.ObterTodosProdutos().Result.Count,
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        Query = query
        //    };
        //}
    }
}
