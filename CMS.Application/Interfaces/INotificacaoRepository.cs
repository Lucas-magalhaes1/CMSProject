using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Application.Interfaces
{
    public interface INotificacaoRepository
    {
        Task<List<Notificacao>> ObterPorUsuarioIdAsync(Guid usuarioId);
        Task AdicionarAsync(Notificacao notificacao);
        Task AtualizarAsync(Notificacao notificacao);
        Task<Notificacao?> ObterPorIdAsync(Guid id);
    }
}