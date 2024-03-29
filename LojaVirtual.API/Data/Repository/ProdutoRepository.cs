﻿using LojaVirtual.API.Models;
using Loja.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using LojaVirtual.API.ViewModels;

namespace LojaVirtual.API.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoContext _context;

        public ProdutoRepository(ProdutoContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public void AdicionarMarca(Marca marca)
        {
            _context.Marca.Add(marca);
        }

        public  void AdicionarProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void AdicionarTipoProduto(TipoProduto tipoProduto)
        {
            _context.TipoProduto.Add(tipoProduto);
        }

        public void AtualizarMarca(Marca marca)
        {
            _context.Marca.Update(marca);
        }

        public void AtualizarProduto(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void AtualizarTipoProduto(TipoProduto tipoProduto)
        {
            _context.TipoProduto.Update(tipoProduto);
        }

        public async Task<Marca> ObterMarcaPorId(Guid id)
        {
            return await _context.Marca.FindAsync(id);
        }

        public async Task<Produto> ObterProdutoPorId(Guid id)
        {
            return await _context.Produtos
                 .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<TipoProduto> ObterTipoProdutoPorId(Guid id)
        {
            return await _context.TipoProduto.FindAsync(id);
        }

        public async Task<List<Marca>> ObterTodasMarcas()
        {
            return await _context.Marca.Where(c => c.removido == false).OrderBy(c => c.Nome).ToListAsync();
        }

        public async Task<List<Produto>> ObterTodosProdutos()
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(m => m.Marca)
                .Include(t => t.TipoProduto)
                .Include(c => c.Cor)
                .Include(c => c.Tamanho)
                .Where(c => c.removido == false).ToListAsync();
        }

        public async Task<List<Produto>> ObterTodosProdutos(int pageSize, int pageIndex, string query = null)
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(m => m.Marca)
                .Include(t => t.TipoProduto)
                .Include(c => c.Cor)
                .Include(c => c.Tamanho)
                .Where(c => c.removido == false)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Produto>> ObterTodosProdutosFiltradosOrderDefault(List<Guid> idCores, List<Guid> idMarcas, List<Guid> idTamanhos, List<Guid> idTipoProdutos, List<int> idGenero, int pageSize, int pageIndex, string query = null)
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(m => m.Marca)
                .Include(t => t.TipoProduto)
                .Include(c => c.Cor)
                .Include(c => c.Tamanho)
                .Where(c => c.removido == false)
                .Where(c => idCores.Contains(c.CorId) || idCores.Count == 0)
                .Where(c => idMarcas.Contains(c.MarcaId) || idMarcas.Count == 0)
                .Where(c => idTamanhos.Contains(c.TamanhoId) || idTamanhos.Count == 0)
                .Where(c => idTipoProdutos.Contains(c.TipoProdutoId) || idTipoProdutos.Count == 0)
                .Where(c => idGenero.Contains((int)c.Genero) || idGenero.Count == 0)
                .Where(c =>
                    c.Nome.ToLower().Contains(query) ||
                    c.Descricao.ToLower().Contains(query) ||
                    c.Marca.Nome.ToLower().Contains(query) ||
                    c.Cor.Nome.ToLower().Contains(query) ||
                    c.Tamanho.Nome.ToLower().Contains(query) ||
                    c.TipoProduto.Nome.ToLower().Contains(query) ||
                    string.IsNullOrEmpty(query))
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Produto>> ObterTodosProdutosFiltradosOrderValorDesc(List<Guid> idCores, List<Guid> idMarcas, List<Guid> idTamanhos, List<Guid> idTipoProdutos, List<int> idGenero, int pageSize, int pageIndex, string query = null)
        {
            return await _context.Produtos
               .AsNoTracking()
               .Include(m => m.Marca)
               .Include(t => t.TipoProduto)
               .Include(c => c.Cor)
               .Include(c => c.Tamanho)
               .Where(c => c.removido == false)
               .Where(c => idCores.Contains(c.CorId) || idCores.Count == 0)
               .Where(c => idMarcas.Contains(c.MarcaId) || idMarcas.Count == 0)
               .Where(c => idTamanhos.Contains(c.TamanhoId) || idTamanhos.Count == 0)
               .Where(c => idTipoProdutos.Contains(c.TipoProdutoId) || idTipoProdutos.Count == 0)
               .Where(c => idGenero.Contains((int)c.Genero) || idGenero.Count == 0)
               .Where(c =>
                   c.Nome.ToLower().Contains(query) ||
                   c.Descricao.ToLower().Contains(query) ||
                   c.Marca.Nome.ToLower().Contains(query) ||
                   c.Cor.Nome.ToLower().Contains(query) ||
                   c.Tamanho.Nome.ToLower().Contains(query) ||
                   c.TipoProduto.Nome.ToLower().Contains(query) ||
                   string.IsNullOrEmpty(query))
               .OrderByDescending(v => v.ValorVenda )
               .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
               .ToListAsync();
        }

        public async Task<List<Produto>> ObterTodosProdutosFiltradosOrderValorCresc(List<Guid> idCores, List<Guid> idMarcas, List<Guid> idTamanhos, List<Guid> idTipoProdutos, List<int> idGenero, int pageSize, int pageIndex, string query = null)
        {
            return await _context.Produtos
              .AsNoTracking()
              .Include(m => m.Marca)
              .Include(t => t.TipoProduto)
              .Include(c => c.Cor)
              .Include(c => c.Tamanho)
              .Where(c => c.removido == false)
              .Where(c => idCores.Contains(c.CorId) || idCores.Count == 0)
              .Where(c => idMarcas.Contains(c.MarcaId) || idMarcas.Count == 0)
              .Where(c => idTamanhos.Contains(c.TamanhoId) || idTamanhos.Count == 0)
              .Where(c => idTipoProdutos.Contains(c.TipoProdutoId) || idTipoProdutos.Count == 0)
              .Where(c => idGenero.Contains((int)c.Genero) || idGenero.Count == 0)
              .Where(c =>
                  c.Nome.ToLower().Contains(query) ||
                  c.Descricao.ToLower().Contains(query) ||
                  c.Marca.Nome.ToLower().Contains(query) ||
                  c.Cor.Nome.ToLower().Contains(query) ||
                  c.Tamanho.Nome.ToLower().Contains(query) ||
                  c.TipoProduto.Nome.ToLower().Contains(query) ||
                  string.IsNullOrEmpty(query))
              .OrderBy(v => v.ValorVenda)
              .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
              .ToListAsync();
        }      

        public async Task<List<TipoProduto>> ObterTodosTipoProduto()
        {
            return await _context.TipoProduto.Where(c => c.removido == false).OrderBy(c => c.Nome).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<Cor>> ObterTodasCores()
        {
            return await _context.Cor.Where(c => c.removido == false).OrderBy(c => c.Nome).ToListAsync();
        }

        public Task<Cor> ObterCorPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AdicionarCor(Cor cor)
        {
            _context.Cor.Add(cor);
        }

        public void AtualizarCor(Cor cor)
        {
            _context.Cor.Update(cor);
        }

        public async Task<List<Tamanho>> ObterTodosTamanhos()
        {
            return await _context.Tamanho.Where(t => t.removido == false).OrderBy(c => c.Nome).ToListAsync();
        }

        public Task<Tamanho> ObterTamanhoPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AdicionarTamanho(Tamanho tamanho)
        {
            _context.Tamanho.Add(tamanho);
        }

        public void AtualizarTamanho(Tamanho tamanho)
        {
            _context.Tamanho.Update(tamanho);
        }       
    } 
}
