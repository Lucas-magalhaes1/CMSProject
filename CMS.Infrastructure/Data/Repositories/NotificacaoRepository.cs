using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Data.Repositories
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly AppDbContext _context;

        public NotificacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notificacao>> ObterPorUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Notificacoes
                .Where(n => n.UsuarioId == usuarioId && !n.Lida)
                .OrderByDescending(n => n.DataCriacao)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Notificacao notificacao)
        {
            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Notificacao notificacao)
        {
            _context.Notificacoes.Update(notificacao);
            await _context.SaveChangesAsync();
        }
        public async Task<Notificacao?> ObterPorIdAsync(Guid id)
        {
            return await _context.Notificacoes.FindAsync(id);
        }

    }
}