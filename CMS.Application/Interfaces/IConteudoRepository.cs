using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Application.Interfaces
{
    public interface IConteudoRepository
    {
        Task<Conteudo> CriarAsync(Conteudo conteudo);
        Task<List<Conteudo>> ListarAsync();
        Task<List<Conteudo>> ListarPorCriadorAsync(Guid usuarioId); 
        Task<Conteudo?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Conteudo conteudo);
        Task DeletarAsync(Conteudo conteudo);
    }
}